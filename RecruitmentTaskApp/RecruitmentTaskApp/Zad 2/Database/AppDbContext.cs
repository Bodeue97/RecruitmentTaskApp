using Microsoft.EntityFrameworkCore;
using RecruitmentTaskApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<VacationPackage> VacationPackages => Set<VacationPackage>();
        public DbSet<Vacation> Vacations => Set<Vacation>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
