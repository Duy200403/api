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
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetAllWithIncludesAsync(params Expression<Func<Employee, object>>[] includes)
        {
            IQueryable<Employee> query = _context.Employees;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<Employee> GetByIdWithIncludesAsync(Guid id, params Expression<Func<Employee, object>>[] includes)
        {
            IQueryable<Employee> query = _context.Employees;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Employee entity)
        {
            await _context.Employees.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }

            if (await HasRelatedRecordsAsync(id))
            {
                throw new InvalidOperationException("Cannot delete Employee because it has associated Shifts. Please delete the Shifts first.");
            }

            _context.Employees.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedRecordsAsync(Guid id)
        {
            return await _context.Shifts.AnyAsync(s => s.EmployeeId == id);
        }
    }
}