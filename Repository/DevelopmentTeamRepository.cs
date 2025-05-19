using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class DevelopmentTeamRepository : IRepository<DevelopmentTeam>
    {
        private readonly AppDbContext _context;

        public DevelopmentTeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DevelopmentTeam>> GetAllAsync()
        {
            return await _context.DevelopmentTeams.ToListAsync();
        }

        public async Task<DevelopmentTeam> GetByIdAsync(Guid id)
        {
            return await _context.DevelopmentTeams.FindAsync(id);
        }

        public async Task<IEnumerable<DevelopmentTeam>> GetAllWithIncludesAsync(params Expression<Func<DevelopmentTeam, object>>[] includes)
        {
            IQueryable<DevelopmentTeam> query = _context.DevelopmentTeams;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<DevelopmentTeam> GetByIdWithIncludesAsync(Guid id, params Expression<Func<DevelopmentTeam, object>>[] includes)
        {
            IQueryable<DevelopmentTeam> query = _context.DevelopmentTeams;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(dt => dt.Id == id);
        }

        public async Task AddAsync(DevelopmentTeam entity)
        {
            await _context.DevelopmentTeams.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DevelopmentTeam entity)
        {
            var existing = await _context.DevelopmentTeams.FindAsync(entity.Id);
            if (existing == null)
                throw new Exception("DevelopmentTeam not found");
            _context.DevelopmentTeams.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"DevelopmentTeam with ID {id} not found.");
            }

            _context.DevelopmentTeams.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedRecordsAsync(Guid id)
        {
            return await Task.FromResult(false);
        }
    }
}