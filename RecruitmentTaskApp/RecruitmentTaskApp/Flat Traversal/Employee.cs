using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Flat_Traversal
{
    public class Employee
    {
        public int Id { get; set; }
        public String Name { get; set; } = String.Empty;
        public int? SuperiorId { get; set; }
        public virtual Employee? Superior { get; set; }


    }
}
