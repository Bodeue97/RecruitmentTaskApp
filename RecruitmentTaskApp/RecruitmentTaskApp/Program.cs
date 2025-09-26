using RecruitmentTaskApp.Flat_Traversal;
using RecruitmentTaskApp.Optimized_tree___memoization;

//Zad 1. 

// W pierwszym rozwiazaniu zastosowalem proste podejscie:
// - kazdy pracownik ma referencje do bezposredniego przełożonego
// - dla kazdego pracownika przechodze po lancuchu przełożonych
//   i zapisuje poziom (rząd) w relacjach (_relations)
// - metoda GetSuperiorRowOfEmployee wyszukuje w tej strukturze poziom przełożonego
// - w konstruktorze (i w RebuildStructure) buduje sie caly flat hierarchy na starcie


var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Jan Kowalski" },
    new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
    new Employee { Id = 3, Name = "Andrzej Abacki", SuperiorId = 2 },
    new Employee { Id = 4, Name = "Piotr Zielinski" }
};


var serviceCase1 = new EmployeesStructureService(employees);


Console.WriteLine("Case 1 Tests:");

Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(2, 1)); //  1
Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(3, 2)); // 1
Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(3, 1)); //  2
Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(4, 3)); // null
Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(4, 1)); // null


employees.Add(new Employee { Id = 5, Name = "New Employee", SuperiorId = 2 });
serviceCase1.RebuildStructure(employees);

Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(5, 1)); // 2
Console.WriteLine(serviceCase1.GetSuperiorRowOfEmployee(5, 2)); // 1


// W drugim rozwiazaniu zastosowalem podejscie bardziej zoptymalizowane :
// - kazdy pracownik ma referencje do bezposredniego przełożonego oraz liste podwladnych
// - konstruktor (lub RebuildStructure) buduje drzewo organizacyjne od korzeni (pracownicy bez przełożonego)
// - metoda BuildAncestorMap rekurencyjnie przechodzi po drzewie
//   i zapisuje dla kazdego pracownika wszystkich jego przełożonych wraz z poziomem (rząd)
// - dzieki temu GetSuperiorRowOfEmployee dziala w O(1), bo korzysta z wczesniej stworzonego slownika (dla przelozonego)
// - podejscie to eliminuje wielokrotne przechodzenie tych samych sciezek w hierarchii


var employees2 = new List<EmployeeOpt>
{
    new EmployeeOpt { Id = 1, Name = "Jan Kowalski" },
    new EmployeeOpt { Id = 2, Name = "Kamil Nowak", SuperiorId = 1 },
    new EmployeeOpt { Id = 3, Name = "Andrzej Abacki", SuperiorId = 2 },
    new EmployeeOpt { Id = 4, Name = "Piotr Zielinski" }
};

var serviceCase2 = new EmployeesStructureOptService(employees2);

Console.WriteLine("Bardziej zoptymalizowane podejscie:");

// Direct and indirect superior queries
Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(2, 1)); //  1
Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(3, 2)); //  1
Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(3, 1)); //  2
Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(4, 3)); //  null
Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(4, 1)); //  null

// Add new employee and rebuild
employees2.Add(new EmployeeOpt { Id = 5, Name = "New Employee", SuperiorId = 2 });
serviceCase2.RebuildStructure(employees2);

Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(5, 1)); //  2
Console.WriteLine(serviceCase2.GetSuperiorRowOfEmployee(5, 2)); //  1


