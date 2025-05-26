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

        [HttpGet("software")]
        public async Task<IActionResult> GetSoftwareReport([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] string status)
        {
            var result = await _context.Softwares
                .Where(s => s.StartDate >= fromDate && s.StartDate <= toDate && s.Status == status)
                .Select(s => new SoftwareReportDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = s.Status
                })
                .ToListAsync();

            return Ok(result);
        }

        // Báo cáo nhân viên theo phần mềm và vai trò
        [HttpGet("software-by-employee")]
        public async Task<IActionResult> GetSoftwareByEmployeeReport()
        {
            // Lấy danh sách tất cả nhân viên đang Active
            var report = await (
                from member in _context.DevelopmentTeamMembers
                join team in _context.DevelopmentTeams on member.DevelopmentTeamId equals team.Id
                join software in _context.Softwares on team.SoftwareId equals software.Id
                where member.Status == "Đang làm việc"
                select new
                {
                    EmployeeId = member.Id,
                    EmployeeName = member.Name,
                    SoftwareId = software.Id,
                    SoftwareName = software.Name,
                    Role = member.Role,
                    Position = member.Position,
                    department = member.Department,
                    SoftwareStartDate = software.StartDate,
                    SoftwareStatus = software.Status
                }
            )
            .OrderBy(x => x.EmployeeName)
            .ThenBy(x => x.SoftwareName)
            .ToListAsync();

            return Ok(report);
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