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
        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));
            if (vacationPackage == null) throw new ArgumentNullException(nameof(vacationPackage));

            var currentYear = DateTime.Now.Year;


            var usedDays = vacations
                .Where(v => v.EmployeeId == employee.Id &&
                            v.DateSince.Year <= currentYear &&
                            v.DateUntil.Year >= currentYear &&
                            v.DateUntil < DateTime.Now)
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
    }
}
