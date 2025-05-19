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
    public class ShiftRepository : IRepository<Shift>
    {
        private readonly AppDbContext _context;

        public ShiftRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shift>> GetAllAsync()
        {
            return await _context.Shifts.ToListAsync();
        }

        public async Task<Shift> GetByIdAsync(Guid id)
        {
            return await _context.Shifts.FindAsync(id);
        }

        public async Task<IEnumerable<Shift>> GetAllWithIncludesAsync(params Expression<Func<Shift, object>>[] includes)
        {
            IQueryable<Shift> query = _context.Shifts;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<Shift> GetByIdWithIncludesAsync(Guid id, params Expression<Func<Shift, object>>[] includes)
        {
            IQueryable<Shift> query = _context.Shifts;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Shift entity)
        {
            await _context.Shifts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shift entity)
        {
        var existingEntity = await _context.Shifts.FindAsync(entity.Id);
        if (existingEntity == null)
            throw new Exception("Shift not found");
        // Cập nhật giá trị
        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync(); 
            // _context.Shifts.Update(entity);
            // await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Shift with ID {id} not found.");
            }

            _context.Shifts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedRecordsAsync(Guid id)
        {
            return await Task.FromResult(false);
        }
    }
}