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
            // Clear tables
            context.Set<Vacation>().RemoveRange(context.Set<Vacation>());
            context.Set<Employee>().RemoveRange(context.Set<Employee>());
            context.Set<Team>().RemoveRange(context.Set<Team>());
            context.Set<VacationPackage>().RemoveRange(context.Set<VacationPackage>());
            context.SaveChanges();

            // Reset identity for SQL Server
            if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Vacations', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Employees', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Teams', RESEED, 0)");
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('VacationPackages', RESEED, 0)");
            }

            var random = new Random();
            var currentYear = DateTime.Now.Year;

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

            // Create employees
            var employees = new List<Employee>();
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

            // Ensure at least 2 employees have 2025 package
            var package2025 = vacationPackages.First(vp => vp.Year == currentYear);
            employees[0].VacationPackageId = package2025.Id;
            employees[1].VacationPackageId = package2025.Id;
            context.SaveChanges();

            // Generate vacations
            var vacations = new List<Vacation>();

            foreach (var emp in employees)
            {
                int vacationCount = random.Next(0, 3);
                for (int j = 0; j < vacationCount; j++)
                {
                    var year = random.Next(2019, 2026);
                    var startMonth = random.Next(1, 12);
                    var startDay = random.Next(1, 25);
                    var lengthDays = random.Next(1, 5);

                    // Ensure valid DateUntil
                    int daysInMonth = DateTime.DaysInMonth(year, startMonth);
                    if (startDay + lengthDays - 1 > daysInMonth)
                        lengthDays = daysInMonth - startDay + 1;

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

            // Add guaranteed current-year vacations for testing
            vacations.Add(new Vacation
            {
                EmployeeId = employees[0].Id,
                DateSince = new DateTime(currentYear, 1, 5),
                DateUntil = new DateTime(currentYear, 1, 10),
                NumberOfHours = 6 * 8,
                IsPartialVacation = false
            });

            vacations.Add(new Vacation
            {
                EmployeeId = employees[1].Id,
                DateSince = new DateTime(currentYear, 2, 1),
                DateUntil = new DateTime(currentYear, 2, 3),
                NumberOfHours = 3 * 8,
                IsPartialVacation = false
            });

            context.Set<Vacation>().AddRange(vacations);
            context.SaveChanges();
        }
    }
}
