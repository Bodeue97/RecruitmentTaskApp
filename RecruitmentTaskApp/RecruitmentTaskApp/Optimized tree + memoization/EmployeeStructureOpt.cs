using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Optimized_tree___memoization
{
    public class EmployeeStructureOpt
    {
        public int EmployeeId { get; set; }

        public Dictionary<int, int> SuperiorLevels { get; set; } = new();
    }
}
