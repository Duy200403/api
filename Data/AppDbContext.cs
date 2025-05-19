using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Software> Softwares { get; set; }
        public DbSet<DevelopmentTeam> DevelopmentTeams { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình giá trị mặc định cho Id
            modelBuilder.Entity<Software>().Property(s => s.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<DevelopmentTeam>().Property(d => d.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Employee>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Shift>().Property(s => s.Id).HasDefaultValueSql("NEWID()");

            // Cấu hình mối quan hệ 1-n: Software - DevelopmentTeam
            modelBuilder.Entity<DevelopmentTeam>()
                .HasOne(d => d.Software)
                .WithMany(s => s.DevelopmentTeams)
                .HasForeignKey(d => d.SoftwareId)
                .OnDelete(DeleteBehavior.Restrict); // Sử dụng Restrict để tránh multiple cascade paths

            // Cấu hình mối quan hệ 1-n: Employee - Shift
            modelBuilder.Entity<Shift>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Shifts)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Sử dụng Restrict để tránh multiple cascade paths
        }
    }
}