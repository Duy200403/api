using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto;
using api.Models;
using api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopmentTeamMemberController : ControllerBase
    {
        private readonly IRepository<DevelopmentTeamMember> _repository;
        private readonly IMapper _mapper;

        public DevelopmentTeamMemberController(IRepository<DevelopmentTeamMember> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDevelopmentTeam = false)
        {
            var members = includeDevelopmentTeam
                ? await _repository.GetAllWithIncludesAsync(m => m.DevelopmentTeam)
                : await _repository.GetAllAsync();

            var dtos = _mapper.Map<IEnumerable<DevelopmentTeamMemberDto>>(members);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(_mapper.Map<DevelopmentTeamMemberDto>(member));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDevelopmentTeamMemberDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = _mapper.Map<DevelopmentTeamMember>(dto);
            member.Id = Guid.NewGuid();
            member.CreatedDate = DateTime.UtcNow;

            await _repository.AddAsync(member);

            return CreatedAtAction(nameof(GetById), new { id = member.Id }, _mapper.Map<DevelopmentTeamMemberDto>(member));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDevelopmentTeamMemberDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            existing.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
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
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
