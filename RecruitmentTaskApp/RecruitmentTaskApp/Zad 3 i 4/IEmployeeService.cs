using RecruitmentTaskApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Zad_3
{
    public interface IEmployeeService
    {
        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage);
        public int CalculateFreeDaysFromAggregate(int grantedDays, int usedDays);

    }
}
