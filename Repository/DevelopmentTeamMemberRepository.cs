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
    public class DevelopmentTeamMemberRepository : IRepository<DevelopmentTeamMember>
    {
        private readonly AppDbContext _context;

        public DevelopmentTeamMemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DevelopmentTeamMember>> GetAllAsync()
        {
            return await _context.DevelopmentTeamMembers
                .Include(m => m.DevelopmentTeam)
                .ToListAsync();
        }

        public async Task<DevelopmentTeamMember> GetByIdAsync(Guid id)
        {
            return await _context.DevelopmentTeamMembers.Include(m => m.DevelopmentTeam).FirstOrDefaultAsync(m => m.Id == id);   
        }

        public async Task<IEnumerable<DevelopmentTeamMember>> GetAllWithIncludesAsync(params Expression<Func<DevelopmentTeamMember, object>>[] includes)
        {
            IQueryable<DevelopmentTeamMember> query = _context.DevelopmentTeamMembers;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<DevelopmentTeamMember> GetByIdWithIncludesAsync(Guid id, params Expression<Func<DevelopmentTeamMember, object>>[] includes)
        {
            IQueryable<DevelopmentTeamMember> query = _context.DevelopmentTeamMembers;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(dt => dt.Id == id);
        }

        public async Task AddAsync(DevelopmentTeamMember entity)
        {
            await _context.DevelopmentTeamMembers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DevelopmentTeamMember entity)
        {
            var existing = await _context.DevelopmentTeamMembers.FindAsync(entity.Id);
            if (existing == null)
                throw new Exception("DevelopmentTeamMember not found");

            _context.DevelopmentTeamMembers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"DevelopmentTeamMember with ID {id} not found.");

            _context.DevelopmentTeamMembers.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedRecordsAsync(Guid id)
        {
            // Nếu có logic kiểm tra liên quan, bổ sung tại đây
            return await Task.FromResult(false);
        }
    }
}
