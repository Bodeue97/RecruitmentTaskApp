using Microsoft.EntityFrameworkCore;
using RecruitmentTaskApp.Entity;
using RecruitmentTaskApp.Zad_2.DTO;
using System;

namespace RecruitmentTaskApp.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<VacationPackage> VacationPackages => Set<VacationPackage>();
        public DbSet<Vacation> Vacations => Set<Vacation>();
        public DbSet<EmployeeVacationSummary> employeeVacationSummaries => Set<EmployeeVacationSummary>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          // Tutaj indeksowanie do Zad 5. strategia 4:
            modelBuilder.Entity<Vacation>()
                .HasIndex(v => v.EmployeeId)
                .HasDatabaseName("IX_Vacations_EmployeeId");

           
            modelBuilder.Entity<Vacation>()
                .HasIndex(v => new { v.EmployeeId, v.DateSince, v.DateUntil })
                .HasDatabaseName("IX_Vacations_EmployeeId_DateRange");

    
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.VacationPackageId)
                .HasDatabaseName("IX_Employees_VacationPackageId");

            modelBuilder.Entity<EmployeeVacationSummary>().HasNoKey();

        }
    }
}
