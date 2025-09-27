using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;    

        public int? TeamId { get; set; }
        public int? VacationPackageId { get; set; }
        //tutaj jest mala rozbieznosc miedzy wizualnym przedstawieniem relacji miedzy tabelami
        // - a opisem tekstowym, gdzie dodane jest positionId
        public int PositionId { get; set; }

        public Team? Team { get; set; }
        public VacationPackage? VacationPackage { get; set; }
        public ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
    }
}
