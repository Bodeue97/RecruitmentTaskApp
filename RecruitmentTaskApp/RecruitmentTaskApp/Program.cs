using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RecruitmentTaskApp.Database;
using RecruitmentTaskApp.Entity;
using RecruitmentTaskApp.Zad_3;


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

var memoryCache = new MemoryCache(new MemoryCacheOptions());
IEmployeeService _es = new EmployeeService(memoryCache);


using var contextLinq = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase("RecruitmentDb")
    .Options);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();


var connectionString = config.GetConnectionString("RecruitmentDb");

using var contextSql = new AppDbContext(
    new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlServer(connectionString)
        .Options
);

contextSql.Database.EnsureCreated();



DbSeeder.Seed(contextSql);


DbSeeder.Seed(contextLinq);
var currentYear = DateTime.Now.Year;
var startOfYear = new DateTime(currentYear, 1, 1);
var endOfYear = new DateTime(currentYear, 12, 31);



Console.WriteLine("Database seeded with sample data!");
#endregion

// I. pobieranie jedynie niezbednych pol z potrzebnych tabel

#region Tylko potrzebne pola kod
//nieoptymalne query
var employeeLinqNonOpt = contextLinq.Employees
    .Include(e => e.Team)
    .Include(e => e.Vacations)
    .Include(e => e.VacationPackage)
    .ToList();



//optymalne query

var employeeLinqOpt = contextLinq.Employees
    .Select(e => new
    {
        Employee = new Employee { Id = e.Id },
        Vacations = e.Vacations
            .Select(v => new Vacation
            {
                EmployeeId = v.EmployeeId,
                DateSince = v.DateSince,
                DateUntil = v.DateUntil
            })
            .ToList(),
        VacationPackage = e.VacationPackage == null ? null : new VacationPackage
        {
            GrantedDays = e.VacationPackage.GrantedDays,
            Year = e.VacationPackage.Year
        }
    })
    .ToList();


foreach (var e in employeeLinqOpt)
{
    Console.WriteLine(_es.CountFreeDaysForEmployee(e.Employee, e!.Vacations, e.VacationPackage!));
}

#endregion


// II. agregowanie danych po stronie bazy - tutaj wyliczamy
// - wszystkie wartosci jeszcze zanim pobierzemy dane z bazy = mniej danych
// - do pobrania = badziej optymalne zapytanie

#region Kod agregowanie po stronie bazy


var employeeVacationDaysAggregated = contextSql.Employees
    .Select(e => new
    {
        EmployeeId = e.Id,
        GrantedDays = e.VacationPackage != null ? e.VacationPackage.GrantedDays : 0,
        PackageYear = e.VacationPackage != null ? e.VacationPackage.Year : 0,
        UsedDays = e.Vacations
            .Where(v => v.DateSince <= endOfYear && v.DateUntil >= startOfYear && v.DateUntil < DateTime.Now)
            .Sum(v =>
                EF.Functions.DateDiffDay(
                    v.DateSince < startOfYear ? startOfYear : v.DateSince,
                    v.DateUntil > endOfYear ? endOfYear : v.DateUntil
                ) + 1
            )
    })
    .ToList();
Console.WriteLine("Aggregated: \n");
foreach (var e in employeeVacationDaysAggregated)
{
    Console.WriteLine(_es.CalculateFreeDaysFromAggregate(e!.GrantedDays, e.UsedDays, e.PackageYear));
}


#endregion

// III. Uzywanie wydajnych joinow w celu ograniczenia czestotliwosci pobierania danych z bazy.
// - tutaj chodzi mi glownie o to zeby pobrac wszystkie potrzebne dane jednym query.
// - Przykladowego kodu nie uwzgledniam w tym punkcie, bo w tym przypadku trzeba sie 
// - naprawde postarac, zeby pobierac rozne dane potrzebne do metody z zad 3 w osobnych 
// - zapytaniach




// IV. Dodawanie odpowiednich indeksów w celu przyspieszenia filtrowania i joinów.
// - dla kolumn, po których często filtrujemy lub łączymy tabele, tworzymy indeksy
// - np. w tabeli Vacations dodajemy indeks na kolumnie EmployeeId
//      CREATE INDEX IX_Vacations_EmployeeId ON Vacations(EmployeeId);
// - dzięki temu zapytania typu "WHERE EmployeeId = x" lub joiny z tabelą Employees
//   nie wymagają pełnego skanowania tabeli, tylko szybkie przejście po strukturze indeksu
// - w SQL Server indeksy są implementowane jako B+ tree
//      - każdy węzeł ma uporządkowane klucze i wskaźniki do dzieci lub do rekordów
//      - pozwala to znaleźć poszukiwany rekord w log(n) porównań zamiast pełnego skanu
// - indeksy klastrowane sortują fizycznie dane tabeli, a nieklastrowane przechowują wskaźniki
// - w ten sposób przy dużych tabelach agregacje, joiny i filtrowanie działają znacznie szybciej
// - Indeksowanie zostalo zaimplementowane w AppDbContext.cs od linii 20.


// V. Korzystanie ze stored procedure do obliczania potrzebnych danych po stronie db - w przypadkach gdy
// - dane liczymy regularnie/wielokrotnie

#region Kod stored procedure przyklad

//var createProcedureSql = @"
//IF OBJECT_ID('GetEmployeeVacationSummary', 'P') IS NULL
//BEGIN
//    EXEC('
//    CREATE PROCEDURE GetEmployeeVacationSummary
//        @EmployeeId INT
//    AS
//    BEGIN
//        SELECT 
//            e.Id AS EmployeeId,
//            vp.GrantedDays,
//            vp.Year AS PackageYear,
//            ISNULL(SUM(DATEDIFF(DAY, v.DateSince, v.DateUntil) + 1), 0) AS UsedDays
//        FROM Employees e
//        LEFT JOIN VacationPackages vp ON e.VacationPackageId = vp.Id
//        LEFT JOIN Vacations v ON v.EmployeeId = e.Id
//        WHERE e.Id = @EmployeeId
//        GROUP BY e.Id, vp.GrantedDays, vp.Year;
//    END
//    ')
//END
//";

//contextSql.Database.ExecuteSqlRaw(createProcedureSql);

//var employeeIdParam = new SqlParameter("@EmployeeId", 1); 

//var summary = contextSql.Set<EmployeeVacationSummary>()
//    .FromSqlRaw("EXEC GetEmployeeVacationSummary @EmployeeId", employeeIdParam)
//    .AsEnumerable() 
//    .FirstOrDefault();

//if (summary != null)
//{
//    int freeDays = _es.CalculateFreeDaysFromAggregate(
//        summary.GrantedDays,
//        summary.UsedDays,
//        summary.PackageYear
//    );

//    Console.WriteLine($"Employee {summary.EmployeeId} has {freeDays} free vacation days.");
//}


#endregion


// VI. Korzystanie z in-memory cache do przechowywania czesto pobieranych danych
// - w celu ograniczenia czestotliwosci odbijania sie od db. Implementacja cache 
// - w EmployeeService.cs line 54. jesli dni pracownika o danym id byly juz obliczane
// - w ciagu ostatnich 10min to dane te sa pobierane z cache, nie z bazy. Tutaj pojawia
// - sie problem gdy dane zostaly zaktualizowane w ciagu ostatnich 10 minut przez co 
// - istnieje ryzyko zwrocenia blednej ilosci dni, ale mozna tego uniknac uwzgledniajac
// - update cache przy skladaniu wniosku o urlop, np usuwajac cashed pozostale dni dla tego pracownika.








