using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dto;
using api.Models;
using api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IRepository<Shift> _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ShiftController(IRepository<Shift> repository, IMapper mapper, AppDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeEmployee = true)
        {
            var shifts = includeEmployee
                ? await _repository.GetAllWithIncludesAsync(s => s.Employee)
                : await _repository.GetAllAsync();
            var shiftDtos = _mapper.Map<IEnumerable<ShiftDto>>(shifts);
            return Ok(shiftDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeEmployee = true)
        {
            var shift = includeEmployee
                ? await _repository.GetByIdWithIncludesAsync(id, s => s.Employee)
                : await _repository.GetByIdAsync(id);
            if (shift == null) return NotFound();
            var shiftDto = _mapper.Map<ShiftDto>(shift);
            return Ok(shiftDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShiftDto shiftDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (shiftDto.EmployeeId == null || !shiftDto.EmployeeId.Any())
                return BadRequest("Danh sách nhân viên không được để trống");

            // Kiểm tra các ID tồn tại
            var existingIds = await _context.Employees
                .Where(e => shiftDto.EmployeeId.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            var invalidIds = shiftDto.EmployeeId.Except(existingIds).ToList();

            if (invalidIds.Any())
                return BadRequest($"EmployeeId {string.Join(", ", invalidIds)} không tồn tại.");

            // Tạo danh sách shift mới cho từng EmployeeId
            var shifts = shiftDto.EmployeeId.Select(empId =>
            {
        var shift = new Shift
        {
            Id = Guid.NewGuid(),
            EmployeeId = empId,
            ShiftDate = shiftDto.ShiftDate,
            StartTime = shiftDto.StartTime,
            EndTime = shiftDto.EndTime,
            CreatedBy = shiftDto.CreatedBy,
            UpdatedBy = shiftDto.UpdatedBy,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
        return shift;
            }).ToList();

            foreach (var shift in shifts)
            {
                await _repository.AddAsync(shift);
            }

            return Ok("Tạo ca làm việc thành công cho các nhân viên.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("report/shift-count")]
        public async Task<IActionResult> GetShiftCount([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var shifts = await _repository.GetAllAsync();
            var shiftCount = shifts
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .GroupBy(s => s.ShiftDate)
                .Select(g => new ShiftCountReportDto { Date = g.Key, Count = g.Count() })
                .ToList();
            return Ok(shiftCount);
        }

        [HttpGet("report/employee-shifts")]
        public async Task<IActionResult> GetEmployeeShifts([FromQuery] Guid employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var shifts = await _repository.GetAllAsync();
            var result = shifts
                .Where(s => s.EmployeeId == employeeId && s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .Select(s => new EmployeeShiftsReportDto
                {
                    Id = s.Id,
                    ShiftDate = s.ShiftDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .ToList();
            return Ok(result);
        }

        [HttpGet("report/employee-shift-total")]
        public async Task<IActionResult> GetEmployeeShiftTotal([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var shifts = await _repository.GetAllAsync();
            var shiftTotals = shifts
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .GroupBy(s => s.EmployeeId)
                .Select(g => new EmployeeShiftTotalReportDto { EmployeeId = g.Key, TotalShifts = g.Count() })
                .ToList();
            return Ok(shiftTotals);
        }

        [HttpGet("report/work-efficiency")]
        public async Task<IActionResult> GetWorkEfficiency([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var shifts = await _repository.GetAllWithIncludesAsync(s => s.Employee);
            var efficiency = shifts
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate)
                .GroupBy(s => s.EmployeeId)
                .Select(g => new WorkEfficiencyReportDto
                {
                    EmployeeId = g.Key,
                    EmployeeName = g.First().Employee?.Name ?? "Unknown",
                    TotalShifts = g.Count(),
                    TotalHours = g.Sum(s => (s.EndTime - s.StartTime).TotalHours)
                })
                .ToList();
            return Ok(efficiency);
        }
    }
}