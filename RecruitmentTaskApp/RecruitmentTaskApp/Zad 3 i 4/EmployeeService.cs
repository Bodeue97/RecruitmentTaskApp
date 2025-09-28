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

        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            int remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);
            return remainingDays > 0;
        }

        public int CalculateFreeDaysFromAggregate(int grantedDays, int usedDays)
        {
            if (grantedDays == 0) return -1; 
            return grantedDays - usedDays;
        }

    }
}
