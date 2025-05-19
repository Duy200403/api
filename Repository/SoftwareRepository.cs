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
    public class SoftwareRepository : IRepository<Software>
    {
        private readonly AppDbContext _context;

        public SoftwareRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Software>> GetAllAsync()
        {
            return await _context.Softwares.ToListAsync();
        }

        public async Task<Software> GetByIdAsync(Guid id)
        {
            return await _context.Softwares.FindAsync(id);
        }

        public async Task<IEnumerable<Software>> GetAllWithIncludesAsync(params Expression<Func<Software, object>>[] includes)
        {
            IQueryable<Software> query = _context.Softwares;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<Software> GetByIdWithIncludesAsync(Guid id, params Expression<Func<Software, object>>[] includes)
        {
            IQueryable<Software> query = _context.Softwares;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Software entity)
        {
            await _context.Softwares.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Software entity)
        {
            _context.Softwares.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Software with ID {id} not found.");
            }

            if (await HasRelatedRecordsAsync(id))
            {
                throw new InvalidOperationException("Cannot delete Software because it has associated DevelopmentTeams. Please delete the DevelopmentTeams first.");
            }

            _context.Softwares.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedRecordsAsync(Guid id)
        {
            return await _context.DevelopmentTeams.AnyAsync(dt => dt.SoftwareId == id);
        }
    }
}