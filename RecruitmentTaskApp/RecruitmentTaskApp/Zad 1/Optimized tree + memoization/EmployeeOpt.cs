using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Optimized_tree___memoization
{
    public class EmployeeOpt
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SuperiorId { get; set; }
        public virtual EmployeeOpt? Superior { get; set; }
        public List<EmployeeOpt> Subordinates { get; set; } = new();
    }
}
