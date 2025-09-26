using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Flat_Traversal
{
    public interface IEmployeesStructureService
    {
        int? GetSuperiorRowOfEmployee(int employeeId, int superiorId);
        void RebuildStructure(List<Employee> employees);
    }
}
