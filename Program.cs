
using apbd_cw_rental_system_s32529.Domain.Equipment;
using apbd_cw_rental_system_s32529.Domain.Users;
using apbd_cw_rental_system_s32529.Exceptions;
using apbd_cw_rental_system_s32529.Policies;
using apbd_cw_rental_system_s32529.Services;

Console.WriteLine("=== ROZPOCZĘCIE SYMULACJI WYPOŻYCZALNI ===");

// Wstrzykujemy zależność: 10 zł za każdy dzień zwłoki
var penaltyCalculator = new StandardPenaltyCalculator(10.0m); 
var rentalService = new RentalService(penaltyCalculator);

// 1. Dodanie kilku egzemplarzy sprzętu
Console.WriteLine("\n--- Dodawanie sprzętu ---");
var laptop = new Laptop("Dell XPS 15", 16, "Intel i7");
var projector = new Projector("Epson 4K", 3000, "1080p");
var camera = new Camera("Sony Alpha", true, "50mm");

rentalService.AddEquipment(laptop);
rentalService.AddEquipment(projector);
rentalService.AddEquipment(camera);
Console.WriteLine("Dodano Laptop, Projektor i Kamerę.");

// 2. Dodanie użytkowników
Console.WriteLine("\n--- Dodawanie użytkowników ---");
var student = new Student("Jan", "Kowalski");
var employee = new Employee("Anna", "Nowak");

rentalService.AddUser(student);
rentalService.AddUser(employee);
Console.WriteLine($"Dodano studenta: {student.FirstName} i pracownika: {employee.FirstName}");

// 3. Poprawne wypożyczenie
Console.WriteLine("\n--- Wypożyczanie sprzętu ---");
var rental1 = rentalService.RentEquipment(student.Id, laptop.Id, TimeSpan.FromDays(5));
var rental2 = rentalService.RentEquipment(student.Id, camera.Id, TimeSpan.FromDays(2));
Console.WriteLine($"Student {student.FirstName} pomyślnie wypożyczył {laptop.Name} na 5 dni oraz {camera.Name} na 2 dni.");

// 4. Próby niepoprawnych operacji (łapanie wyjątków domenowych)
Console.WriteLine("\n--- Próby niepoprawnych operacji ---");
try
{
    Console.WriteLine($"-> Próba wypożyczenia {projector.Name} przez {student.FirstName} (który ma już 2 sprzęty)...");
    rentalService.RentEquipment(student.Id, projector.Id, TimeSpan.FromDays(1));
}
catch (DomainException ex)
{
    Console.WriteLine($"[ZABLOKOWANO]: {ex.Message}");
}

try
{
    Console.WriteLine($"-> Próba wypożyczenia niedostępnego sprzętu ({laptop.Name}) przez {employee.FirstName}...");
    rentalService.RentEquipment(employee.Id, laptop.Id, TimeSpan.FromDays(1));
}
catch (DomainException ex)
{
    Console.WriteLine($"[ZABLOKOWANO]: {ex.Message}");
}

// 5 & 6. Zwrot w terminie i zwrot opóźniony z karą
Console.WriteLine("\n--- Zwroty sprzętu ---");
rentalService.ReturnEquipment(rental1.Id, DateTime.Now); // Zwraca od razu
Console.WriteLine($"{laptop.Name} zwrócony w terminie. Naliczone kary: {rental1.PenaltyFee} zł.");

// Symulujemy zwrot po 7 dniach (wypożyczone na 2 dni, więc 5 dni spóźnienia * 10zł = 50zł kary)
rentalService.ReturnEquipment(rental2.Id, DateTime.Now.AddDays(7)); 
Console.WriteLine($"{camera.Name} zwrócony po terminie! Naliczone kary: {rental2.PenaltyFee} zł.");

// 7. Raport końcowy
Console.WriteLine("\n--- Raport końcowy systemu ---");
var report = rentalService.GenerateSummaryReport();
Console.WriteLine($"- Całkowity sprzęt w systemie: {report.TotalEquipment}");
Console.WriteLine($"- Sprzęt gotowy do wzięcia:    {report.AvailableEquipment}");
Console.WriteLine($"- Aktywne wypożyczenia:        {report.ActiveRentals}");
Console.WriteLine($"- Przeterminowane zwroty:      {report.OverdueRentals}");

Console.WriteLine("\n=== KONIEC SYMULACJI ===");