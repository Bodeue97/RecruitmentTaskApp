using Microsoft.Extensions.Caching.Memory;
using RecruitmentTaskApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Zad_3
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        public EmployeeService(IMemoryCache cache)
        {
            _cache = cache;
        }
        //Tutaj chyba bardziej optymalnie byloby przekazywac tylko Employee ktory zawiera juz reszte argumentow potrzebnych w metodzie
        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            ArgumentNullException.ThrowIfNull(employee);
            ArgumentNullException.ThrowIfNull(vacationPackage);

            var currentYear = DateTime.Now.Year;

  
            if (vacationPackage.Year != currentYear)
                return -1;
            var usedDays = vacations
                   .Where(v => v.EmployeeId == employee.Id &&
                              
                               v.DateSince.Year <= currentYear &&
                               v.DateUntil.Year >= currentYear)
                   .Sum(v =>
                   {
                 
                       var start = v.DateSince.Year < currentYear ? new DateTime(currentYear, 1, 1) : v.DateSince;
                       var end = v.DateUntil.Year > currentYear ? new DateTime(currentYear, 12, 31) : v.DateUntil;
                       return (int)((end - start).TotalDays + 1);
                   });
            return vacationPackage.GrantedDays - usedDays;

        }

        // Overloaded metoda z jednym argumentem oraz uzywanie cache Zad6. strat VI.
        public int CountFreeDaysForEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);


            var cacheKey = $"FreeDays_{employee.Id}";
            if (_cache.TryGetValue(cacheKey, out int cachedFreeDays))
            {
                return cachedFreeDays;
            }

            var vacationPackage = employee.VacationPackage;
            if (vacationPackage == null) return -1;

            var currentYear = DateTime.Now.Year;

            if (vacationPackage.Year != currentYear)
                return -1;

            var usedDays = employee.Vacations
                .Where(v => v.DateSince.Year <= currentYear && v.DateUntil.Year >= currentYear)
                .Sum(v =>
                {
                    var start = v.DateSince.Year < currentYear ? new DateTime(currentYear, 1, 1) : v.DateSince;
                    var end = v.DateUntil.Year > currentYear ? new DateTime(currentYear, 12, 31) : v.DateUntil;
                    return (int)((end - start).TotalDays + 1);
                });

            int freeDays = vacationPackage.GrantedDays - usedDays;

     
            _cache.Set(cacheKey, freeDays, _cacheDuration);

            return freeDays;
        }





        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            int remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);
            return remainingDays > 0;
        }

        public int CalculateFreeDaysFromAggregate(int grantedDays, int usedDays, int packageYear)
        {
            var currentYear = DateTime.Now.Year;

            if (packageYear != currentYear)
                return -1;

            return grantedDays - usedDays;
        }


    }
}
