using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Optimized_tree___memoization
{
    internal interface IEmployeeStructureOptService
    {
        int? GetSuperiorRowOfEmployee(int employeeId, int superiorId);

        void RebuildStructure(List<EmployeeOpt> employees);
    }
}
