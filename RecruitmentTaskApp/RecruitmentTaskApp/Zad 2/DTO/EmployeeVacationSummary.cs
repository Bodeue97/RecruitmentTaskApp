using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Zad_2.DTO
{
    public class EmployeeVacationSummary
    {
        public int EmployeeId { get; set; }
        public int GrantedDays { get; set; }
        public int PackageYear { get; set; }
        public int UsedDays { get; set; }
    }

}
