using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace api.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Software> Softwares { get; set; }
        public DbSet<DevelopmentTeam> DevelopmentTeams { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<DevelopmentTeamMember> DevelopmentTeamMembers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 👈 THÊM DÒNG NÀY để tránh lỗi khi kế thừa từ IdentityDbContext
            // Cấu hình giá trị mặc định cho Id
            modelBuilder.Entity<Software>().Property(s => s.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<DevelopmentTeam>().Property(d => d.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Employee>().Property(e => e.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Shift>().Property(s => s.Id).HasDefaultValueSql("NEWID()");

            // One-to-Many: DevelopmentTeam - DevelopmentTeamMember
            modelBuilder.Entity<DevelopmentTeamMember>()
                .HasOne(m => m.DevelopmentTeam)
                .WithMany(dt => dt.Members)
                .HasForeignKey(m => m.DevelopmentTeamId)
                .OnDelete(DeleteBehavior.Cascade);
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

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Staff", NormalizedName = "STAFF" },
                new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}