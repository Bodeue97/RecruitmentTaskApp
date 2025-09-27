using NUnit.Framework;
using RecruitmentTaskApp.Entity;
using RecruitmentTaskApp.Zad_3;
using System;
using System.Collections.Generic;

namespace RecruitmentTaskApp.Tests
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private IEmployeeService _service;

        [SetUp]
        public void Setup()
        {
            _service = new EmployeeService();
        }

        [Test]
        public void Employee_Can_Request_Vacation()
        {
        
            var employee = new Employee { Id = 1, Name = "Alice" };
            var vacationPackage = new VacationPackage { GrantedDays = 10 };
            var vacations = new List<Vacation>
            {
                new Vacation { EmployeeId = 1, DateSince = DateTime.Now.AddDays(-5), DateUntil = DateTime.Now.AddDays(-3) }
            };

         
            var canRequest = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            Assert.That(canRequest, Is.True, "Employee should be able to request vacation when there are remaining days.");
        }

        [Test]
        public void Employee_Cannot_Request_Vacation()
        {
          
            var employee = new Employee { Id = 2, Name = "Bob" };
            var vacationPackage = new VacationPackage { GrantedDays = 2 };
            var vacations = new List<Vacation>
            {
                new Vacation { EmployeeId = 2, DateSince = DateTime.Now.AddDays(-3), DateUntil = DateTime.Now.AddDays(-1) }
            };

            var canRequest = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            Assert.That(canRequest, Is.False, "Employee should NOT be able to request vacation when no days are left.");
        }

        [Test]
        public void Employee_With_No_Vacations_Can_Request_Vacation()
        {
            var employee = new Employee { Id = 3, Name = "Charlie" };
            var vacationPackage = new VacationPackage { GrantedDays = 15 };
            var vacations = new List<Vacation>(); 

            var canRequest = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            Assert.That(canRequest, Is.True, "Employee with no vacations should be able to request vacation.");
        }

        [Test]
        public void Employee_With_Future_Vacations_Can_Request_Vacation()
        {
            var employee = new Employee { Id = 4, Name = "Diana" };
            var vacationPackage = new VacationPackage { GrantedDays = 10 };
            var vacations = new List<Vacation>
            {
                new Vacation
                {
                    EmployeeId = 4,
                    DateSince = DateTime.Now.AddDays(5),   
                    DateUntil = DateTime.Now.AddDays(10)   
                }
            };

            var canRequest = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            Assert.That(canRequest, Is.True, "Future vacations should not reduce available vacation days.");
        }

        [Test]
        public void Employee_With_Vacation_Spanning_Years_Counts_Correctly()
        {
            var employee = new Employee { Id = 5, Name = "Edward" };
            var vacationPackage = new VacationPackage { GrantedDays = 10 };
            var currentYear = DateTime.Now.Year;

            var vacations = new List<Vacation>
            {
                new Vacation
                {
                    EmployeeId = 5,
                    DateSince = new DateTime(currentYear - 1, 12, 29),
                    DateUntil = new DateTime(currentYear, 1, 2)        
                }
            };

            var remainingDays = _service.CountFreeDaysForEmployee(employee, vacations, vacationPackage);

            Assert.That(remainingDays, Is.EqualTo(8), "Only days within the current year should be subtracted.");
        }
    }
}
