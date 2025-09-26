using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTaskApp.Optimized_tree___memoization
{
    public class EmployeesStructureOptService : IEmployeeStructureOptService
    {
        private readonly Dictionary<int, EmployeeStructureOpt> _employeeStructures = new();

        public EmployeesStructureOptService(List<EmployeeOpt> employees)
        {
            BuildStructure(employees);
        }

        private void BuildStructure(List<EmployeeOpt> employees)
        {
            _employeeStructures.Clear();

      
            var employeeDict = employees.ToDictionary(e => e.Id);

       
            foreach (var e in employees)
            {
                e.Subordinates.Clear();
                if (e.SuperiorId.HasValue)
                {
                    var superior = employeeDict[e.SuperiorId.Value];
                    e.Superior = superior;
                    superior.Subordinates.Add(e);
                }
                else
                {
                    e.Superior = null;
                }
            }

            foreach (var e in employees.Where(e => e.Superior == null))
            {
                BuildAncestorMap(e);
            }
        }

        private void BuildAncestorMap(EmployeeOpt employee)
        {
            if (!_employeeStructures.ContainsKey(employee.Id))
                _employeeStructures[employee.Id] = new EmployeeStructureOpt { EmployeeId = employee.Id };

            var currentStructure = _employeeStructures[employee.Id];

            if (employee.Superior != null)
            {
                var superiorStruct = _employeeStructures[employee.Superior.Id];

                foreach (var kv in superiorStruct.SuperiorLevels)
                    currentStructure.SuperiorLevels[kv.Key] = kv.Value + 1;

                currentStructure.SuperiorLevels[employee.Superior.Id] = 1;
            }

            foreach (var sub in employee.Subordinates)
                BuildAncestorMap(sub);
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            if (_employeeStructures.TryGetValue(employeeId, out var structure) &&
                structure.SuperiorLevels.TryGetValue(superiorId, out var level))
            {
                return level;
            }

            return null;
        }
        public void RebuildStructure(List<EmployeeOpt> employees)
        {
            BuildStructure(employees);
        }
    }
}
