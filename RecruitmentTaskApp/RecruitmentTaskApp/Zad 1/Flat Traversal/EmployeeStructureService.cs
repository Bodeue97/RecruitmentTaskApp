using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecruitmentTaskApp.Flat_Traversal
{
    public class EmployeesStructureService : IEmployeesStructureService
    {
        private readonly List<EmployeeStructure> _relations = new();

        public EmployeesStructureService(List<EmployeeFlat> employees)
        {
            BuildStructure(employees);
        }

        private void BuildStructure(List<EmployeeFlat> employees)
        {
            _relations.Clear();

 
            var dict = employees.ToDictionary(e => e.Id);

        
            foreach (var e in employees)
            {
                e.Superior = e.SuperiorId.HasValue ? dict[e.SuperiorId.Value] : null;
            }

            foreach (var e in employees)
            {
                int level = 1;
                var current = e.Superior;
                while (current != null)
                {
                    _relations.Add(new EmployeeStructure
                    {
                        EmployeeId = e.Id,
                        SuperiorId = current.Id,
                        Level = level
                    });

                    current = current.Superior;
                    level++;
                }
            }
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            var rel = _relations
                .FirstOrDefault(r => r.EmployeeId == employeeId && r.SuperiorId == superiorId);

            return rel?.Level;
        }

        public void RebuildStructure(List<EmployeeFlat> employees)
        {
            BuildStructure(employees);
        }
    }
}
