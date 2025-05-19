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
    public class DevelopmentTeamController : ControllerBase
    {
        private readonly IRepository<DevelopmentTeam> _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public DevelopmentTeamController(IRepository<DevelopmentTeam> repository, IMapper mapper, AppDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeSoftware = true)
        {
            var teams = includeSoftware
                ? await _repository.GetAllWithIncludesAsync(dt => dt.Software)
                : await _repository.GetAllAsync();
            var teamDtos = _mapper.Map<IEnumerable<DevelopmentTeamDto>>(teams);
            return Ok(teamDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeSoftware = false)
        {
            var team = includeSoftware
                ? await _repository.GetByIdWithIncludesAsync(id, dt => dt.Software)
                : await _repository.GetByIdAsync(id);
            if (team == null) return NotFound();
            var teamDto = _mapper.Map<DevelopmentTeamDto>(team);
            return Ok(teamDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDevelopmentTeamDto teamDto)
        {
            // if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            // Kiểm tra xem ProjectId có tồn tại không (giả sử bạn có Project liên kết)
            var projectExists = await _context.Softwares.AnyAsync(e => e.Id == teamDto.SoftwareId);
            if (!projectExists)
            {
                return BadRequest($"SoftwareId {teamDto.Id} không tồn tại.");
            }
            var team = _mapper.Map<DevelopmentTeam>(teamDto);
            team.CreatedDate = DateTime.UtcNow;
            await _repository.AddAsync(team);
            var createdDto = _mapper.Map<DevelopmentTeamDto>(team);
            return CreatedAtAction(nameof(GetById), new { id = team.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDevelopmentTeamDto teamDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();
            _mapper.Map(teamDto, existing);
            // var team = _mapper.Map<DevelopmentTeam>(teamDto);
            // team.Id = id;
            // team.UpdatedDate = DateTime.UtcNow;
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}