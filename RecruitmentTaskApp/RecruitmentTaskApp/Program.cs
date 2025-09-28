using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecruitmentTaskApp.Database;
using RecruitmentTaskApp.Entity;
using RecruitmentTaskApp.Zad_3;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


//Zad 1. 

// W pierwszym rozwiazaniu zastosowalem proste podejscie:
// - kazdy pracownik ma referencje do bezposredniego przełożonego
// - dla kazdego pracownika przechodze po lancuchu przełożonych
// - i zapisuje poziom (rząd) w relacjach (_relations)
// - metoda GetSuperiorRowOfEmployee wyszukuje w tej strukturze poziom przełożonego
// - w konstruktorze (i w RebuildStructure) buduje sie caly flat hierarchy na starcie

#region Kod zadanie 1a


//var employees = new List<EmployeeFlat>
//{
//    new EmployeeFlat { Id = 1, Name = "Jan Kowalski" },
//    new EmployeeFlat { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
//    new EmployeeFlat { Id = 3, Name = "Andrzej Abacki", SuperiorId = 2 },
//    new EmployeeFlat { Id = 4, Name = "Piotr Zielinski" }
//};


//var serviceCase1 = new EmployeesStructureService(employees);


//Console.WriteLine("Case 1 Tests:");

//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(2, 1)); //  1
//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(3, 2)); // 1
//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(3, 1)); //  2
//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(4, 3)); // null
//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(4, 1)); // null


//employees.Add(new EmployeeFlat { Id = 5, Name = "New Employee", SuperiorId = 2 });
//serviceCase1.RebuildStructure(employees);

//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(5, 1)); // 2
//Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(5, 2)); // 1

#endregion


// W drugim rozwiazaniu zastosowalem podejscie bardziej zoptymalizowane :
// - kazdy pracownik ma referencje do bezposredniego przełożonego oraz liste podwladnych
// - konstruktor (lub RebuildStructure) buduje drzewo organizacyjne od korzeni (pracownicy bez przełożonego)
// - metoda BuildAncestorMap rekurencyjnie przechodzi po drzewie
// - i zapisuje dla kazdego pracownika wszystkich jego przełożonych wraz z poziomem (rząd)
// - dzieki temu GetSuperiorRowOfEmployee dziala w O(1), bo korzysta z wczesniej stworzonego slownika (dla przelozonego)
// - podejscie to eliminuje wielokrotne przechodzenie tych samych sciezek w hierarchii


#region Kod zadanie 1b


//var employees2 = new List<EmployeeOpt>
//{
//    new EmployeeOpt { Id = 1, Name = "Jan Kowalski" },
//    new EmployeeOpt { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
//    new EmployeeOpt { Id = 3, Name = "Andrzej Abacki", SuperiorId = 2 },
//    new EmployeeOpt { Id = 4, Name = "Piotr Zielinski" }
//};

//var serviceCase2 = new EmployeesStructureOptService(employees2);

//Console.WriteLine("Bardziej zoptymalizowane podejscie:");


//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(2, 1)); //  1
//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(3, 2)); //  1
//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(3, 1)); //  2
//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(4, 3)); //  null
//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(4, 1)); //  null


//employees2.Add(new EmployeeOpt { Id = 5, Name = "New Employee", SuperiorId = 2 });
//serviceCase2.RebuildStructure(employees2);

//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(5, 1)); //  2
//Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(5, 2)); //  1

#endregion



//Zad 2. 

// W drugim zadaniu korzystam z in-memory db ze wzgledu na szybszy setup i zamiar korzystania tylko z linq,
// - ale potem zdecydowalem sie
// - napisac rowniez query w SQL i chcialem faktycznie zaimplementowac wyciaganie danych z db wiec
// - dodalem setup do polaczenia z sql server oraz migracje do db
// - dane seedowane sa w DbSeeder 

//Db setup:
//in-memory dla ef linq 

#region Kod LINQ queries dla in-memory db

//using var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
//    .UseInMemoryDatabase("RecruitmentDb")
//    .Options);


//DbSeeder.Seed(context);


//var employeesNet2019 = context.Employees.Where(e => e.Team != null
//                        && e.Team.Name == ".NET"
//                        && e.Vacations
//                        .Any(v => v.DateSince.Year == 2019 || v.DateUntil.Year == 2019))
//                        .OrderBy(e => e.Id)
//                        .ToList();

//var currentYear = DateTime.Now.Year;
//var employeesCurrentYearUsedVacation = context.Employees
//    .Select(e => new
//    {
//        Employee = e,
//        UsedDays = e.Vacations != null
//            ? e.Vacations
//                .Where(v => v.DateSince.Year < currentYear)
//                .Sum(v => (v.DateUntil - v.DateSince).TotalDays + 1)
//            : 0
//    })
//    .Where(v => v.UsedDays > 0)
//    .OrderBy(e => e.Employee.Id)
//    .ToList();



//var teamsNo2019Vacation = context.Teams
//    .Where(t => t.Employees != null &&
//                t.Employees.All(e => e.Vacations == null
//                || e.Vacations.All(v => v.DateSince.Year != 2019 && v.DateUntil.Year != 2019)))
//                .OrderBy(e => e.Id)
//                .ToList();


//Console.WriteLine("Employees in .NET team with vacation used in 2019 (LINQ):");
//foreach (var e in employeesNet2019)
//{
//    Console.WriteLine($"Id: {e.Id}, Name: {e.Name}");
//}

//Console.WriteLine("\nEmployees that used vacation days in current year (LINQ):");
//foreach (var e in employeesCurrentYearUsedVacation)
//{
//    Console.WriteLine($"Id: {e.Employee.Id}, Name: {e.Employee.Name}, UsedDays: {e.UsedDays}");
//}

//Console.WriteLine("\nTeams with no vacation used in 2019 (LINQ):");
//foreach (var t in teamsNo2019Vacation)
//{
//    Console.WriteLine($"Id: {t.Id}, Name: {t.Name}");
//}

#endregion


//setup sql server dla zapytan raw sql

#region Kod SQL queries dla SQL Server db


//var config = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();


//var connectionString = config.GetConnectionString("RecruitmentDb");

//using var contextSql = new AppDbContext(
//    new DbContextOptionsBuilder<AppDbContext>()
//        .UseSqlServer(connectionString)
//        .Options
//);

//contextSql.Database.EnsureCreated();



//DbSeeder.Seed(contextSql);



//Console.WriteLine("Database seeded with sample data!");


//var employeesNet2019SQL = contextSql.Employees
//    .FromSqlRaw(@"
//       SELECT DISTINCT e.*
//        FROM Employees e
//        JOIN Teams t ON e.TeamId = t.Id
//        JOIN Vacations v ON v.EmployeeId = e.Id
//        WHERE t.Name = '.NET'
//        AND (YEAR(v.DateSince) = 2019 OR YEAR(v.DateUntil) = 2019)

//    ")
//    .ToList();



//var employeesCurrentYearUsedVacationSQL = contextSql
//    .Database
//    .SqlQueryRaw<EmployeeVacationDto>(@"
//        SELECT e.Id, e.Name, ISNULL(SUM(DATEDIFF(day, v.DateSince, v.DateUntil) + 1), 0) AS UsedDays
//        FROM Employees e
//        LEFT JOIN Vacations v ON v.EmployeeId = e.Id
//        WHERE v.DateSince < GETDATE()
//        GROUP BY e.Id, e.Name
//    ")
//    .ToList();



//var teamsNo2019VacationSQL = contextSql.Teams.FromSqlRaw(
//    @"
//    SELECT t.Id, t.Name
//    FROM Teams t
//    WHERE NOT EXISTS (
//        SELECT 1
//     FROM Employees e
//     JOIN Vacations v ON v.EmployeeId = e.Id
//      WHERE e.TeamId = t.Id
//         AND (YEAR(v.DateSince) = 2019 OR YEAR(v.DateUntil) = 2019)
//    );
//    "
//    ).ToList();



//Console.WriteLine("\nEmployees in .NET team with vacation in 2019 (SQL):");
//foreach (var e in employeesNet2019SQL)
//{
//    Console.WriteLine($"Id: {e.Id}, Name: {e.Name}");
//}



//Console.WriteLine("\nEmployees used vacation days in current year (SQL):");
//foreach (var e in employeesCurrentYearUsedVacationSQL)
//{
//    Console.WriteLine($"Id: {e.Id}, Name: {e.Name}, UsedDays: {e.UsedDays}");
//}



//Console.WriteLine("\nTeams with no vacation in 2019 (SQL):");
//foreach (var t in teamsNo2019VacationSQL)
//{
//    Console.WriteLine($"Id: {t.Id}, Name: {t.Name}");
//}

#endregion

//Zad 3 oraz 4 w odpowiednim folderze

//Zad 5 jest w osobnym projekcie testowym

//Zad 6.

// Tutaj zaprezentuje 6 sposobow na optymalizacje pobierania danych z bazy


#region Context, seeding i service do zad 6.

IEmployeeService _es = new EmployeeService();


using var contextLinq = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase("RecruitmentDb")
    .Options);


DbSeeder.Seed(contextLinq);
var currentYear = DateTime.Now.Year;



Console.WriteLine("Database seeded with sample data!");
#endregion

// I. pobieranie jedynie niezbednych pol z potrzebnych tabel

#region Tylko potrzebne pola kod
////nieoptymalne query
//var employeeLinqNonOpt = contextLinq.Employees
//    .Include(e => e.Team)
//    .Include(e => e.Vacations)
//    .Include(e => e.VacationPackage)
//    .ToList();



////optymalne query

//var employeeLinqOpt = contextLinq.Employees
//    .Select(e => new
//    {
//        Employee = new Employee { Id = e.Id },
//        Vacations = e.Vacations
//            .Select(v => new Vacation
//            {
//                EmployeeId = v.EmployeeId,   
//                DateSince = v.DateSince,
//                DateUntil = v.DateUntil
//            })
//            .ToList(),
//        VacationPackage = e.VacationPackage == null ? null : new VacationPackage
//        {
//            GrantedDays = e.VacationPackage.GrantedDays,
//            Year = e.VacationPackage.Year
//        }
//    })
//    .ToList();


//foreach (var e in employeeLinqOpt)
//{
//    Console.WriteLine(_es.CountFreeDaysForEmployee(e.Employee, e!.Vacations, e.VacationPackage));
//}

#endregion


// II. agregowanie danych po stronie bazy - tuta

var employeeVacationAggregates = contextLinq.Employees
    .Where(e => e.Id == 1)
    .Select(e => new
    {
        GrantedDays = e.VacationPackage != null && e.VacationPackage.Year == currentYear
            ? e.VacationPackage.GrantedDays
            : 0,
        UsedDays = e.Vacations
            .Where(v => v.DateSince.Year <= currentYear && v.DateUntil.Year >= currentYear)
            .Sum(v => /* calculation */)
    })
    .ToList();






