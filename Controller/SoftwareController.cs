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
    public class SoftwareController : ControllerBase
    {
        private readonly IRepository<Software> _repository;
        private readonly IMapper _mapper;

        public SoftwareController(IRepository<Software> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDevelopmentTeams = false)
        {
            var softwares = includeDevelopmentTeams
                ? await _repository.GetAllWithIncludesAsync(s => s.DevelopmentTeams)
                : await _repository.GetAllAsync();
            var softwareDtos = _mapper.Map<IEnumerable<SoftwareDto>>(softwares);
            return Ok(softwareDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeDevelopmentTeams = false)
        {
            var software = includeDevelopmentTeams
                ? await _repository.GetByIdWithIncludesAsync(id, s => s.DevelopmentTeams)
                : await _repository.GetByIdAsync(id);
            if (software == null) return NotFound();
            var softwareDto = _mapper.Map<SoftwareDto>(software);
            return Ok(softwareDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SoftwareDto softwareDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var software = _mapper.Map<Software>(softwareDto);
            software.CreatedDate = DateTime.UtcNow;
            await _repository.AddAsync(software);
            var createdDto = _mapper.Map<SoftwareDto>(software);
            return CreatedAtAction(nameof(GetById), new { id = software.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSoftwareDto softwareDto)
        {
            if (id != softwareDto.Id) return BadRequest();
            var software = _mapper.Map<Software>(softwareDto);
            software.UpdatedDate = DateTime.UtcNow;
            await _repository.UpdateAsync(software);
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