using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Entity
{
    public class VacationPackage
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int GrantedDays { get; set; }
        public int Year { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
