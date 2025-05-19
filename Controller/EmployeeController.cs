using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Models;
using api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        private readonly IMapper _mapper;

        public EmployeeController(IRepository<Employee> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeShifts = false)
        {
            var employees = includeShifts
                ? await _repository.GetAllWithIncludesAsync(e => e.Shifts)
                : await _repository.GetAllAsync();
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeShifts = false)
        {
            var employee = includeShifts
                ? await _repository.GetByIdWithIncludesAsync(id, e => e.Shifts)
                : await _repository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var employee = _mapper.Map<Employee>(employeeDto);
            employee.CreatedDate = DateTime.UtcNow;
            await _repository.AddAsync(employee);
            var createdDto = _mapper.Map<CreateEmployeeDto>(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeUpdateDto employeeDto)
        {
            if (id != employeeDto.Id) return BadRequest();
            var employee = _mapper.Map<Employee>(employeeDto);
            employee.UpdatedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}