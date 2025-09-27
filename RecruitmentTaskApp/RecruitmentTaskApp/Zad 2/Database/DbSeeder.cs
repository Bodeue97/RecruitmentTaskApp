using RecruitmentTaskApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecruitmentTaskApp.Database
{
    public static class DbSeeder
    {
        public static void Seed(DbContext context)
        {
            // Clear existing data
            context.Set<Vacation>().RemoveRange(context.Set<Vacation>());
            context.Set<Employee>().RemoveRange(context.Set<Employee>());
            context.Set<Team>().RemoveRange(context.Set<Team>());
            context.Set<VacationPackage>().RemoveRange(context.Set<VacationPackage>());
            context.SaveChanges();

            var provider = context.Database.ProviderName;
            if (provider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Vacations', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Employees', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Teams', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('VacationPackages', RESEED, 0)");
            }

            // Create teams
            var teams = new List<Team>
            {
                new Team { Name = ".NET" },
                new Team { Name = "Java" },
                new Team { Name = "Python" },
                new Team { Name = "QA" },
            };
            context.Set<Team>().AddRange(teams);

            // Create vacation packages
            var vacationPackages = new List<VacationPackage>
            {
                new VacationPackage { Name = "Standard 2025", GrantedDays = 20, Year = 2025 },
                new VacationPackage { Name = "Extended 2024", GrantedDays = 25, Year = 2024 },
                new VacationPackage { Name = "Standard 2023", GrantedDays = 20, Year = 2023 },
            };
            context.Set<VacationPackage>().AddRange(vacationPackages);
            context.SaveChanges();

            var random = new Random();
            var employees = new List<Employee>();

            // Create employees
            foreach (var team in teams)
            {
                for (int i = 0; i < 5; i++)
                {
                    var vacationPackage = vacationPackages[random.Next(vacationPackages.Count)];
                    employees.Add(new Employee
                    {
                        Name = $"{team.Name}_Employee_{i + 1}",
                        TeamId = team.Id,
                        VacationPackageId = vacationPackage.Id,
                        PositionId = random.Next(1, 5)
                    });
                }
            }

            context.Set<Employee>().AddRange(employees);
            context.SaveChanges();

            // Fix: use TeamId instead of Team navigation property
            var netTeamId = teams.First(t => t.Name == ".NET").Id;
            var otherTeamId = teams.First(t => t.Name != ".NET").Id;

            var netEmployee2019 = employees.First(e => e.TeamId == netTeamId);
            var pastVacationEmployee = employees.First(e => e.TeamId == otherTeamId);

            var teamWithout2019 = teams.First(t => t.Name == "QA");

            // Generate random vacations for employees
            var vacations = new List<Vacation>();
            foreach (var emp in employees)
            {
                int vacationCount = random.Next(0, 3);
                for (int j = 0; j < vacationCount; j++)
                {
                    var year = random.Next(2019, 2026);

                    if (emp.TeamId == teamWithout2019.Id && year == 2019) year = 2020;

                    var startMonth = random.Next(1, 12);
                    var startDay = random.Next(1, 25);
                    var lengthDays = random.Next(1, 5);

                    vacations.Add(new Vacation
                    {
                        EmployeeId = emp.Id,
                        DateSince = new DateTime(year, startMonth, startDay),
                        DateUntil = new DateTime(year, startMonth, startDay + lengthDays - 1),
                        NumberOfHours = lengthDays * 8,
                        IsPartialVacation = false
                    });
                }
            }

            // Ensure specific vacations exist
            vacations.Add(new Vacation
            {
                EmployeeId = netEmployee2019.Id,
                DateSince = new DateTime(2019, 6, 1),
                DateUntil = new DateTime(2019, 6, 5),
                NumberOfHours = 5 * 8,
                IsPartialVacation = false
            });

            vacations.Add(new Vacation
            {
                EmployeeId = pastVacationEmployee.Id,
                DateSince = new DateTime(DateTime.Now.Year - 1, 7, 1),
                DateUntil = new DateTime(DateTime.Now.Year - 1, 7, 3),
                NumberOfHours = 3 * 8,
                IsPartialVacation = false
            });

            context.Set<Vacation>().AddRange(vacations);
            context.SaveChanges();
        }
    }
}
