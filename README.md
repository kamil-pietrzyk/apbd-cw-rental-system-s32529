# Wypożyczalnia Sprzętu Uczelnianego (Projekt C#)

Projekt obiektowy realizujący system zarządzania uczelnianą wypożyczalnią sprzętu. System obsługuje dodawanie urządzeń i użytkowników, proces wypożyczeń i zwrotów, a także egzekwowanie limitów i naliczanie kar.

## Uruchomienie projektu
1. Pobierz repozytorium.
2. Przejdź do głównego katalogu projektu.
3. W terminalu uruchom polecenie: `dotnet run` (lub uruchom projekt w IDE np. Rider/Visual Studio).

## Architektura i Decyzje Projektowe

Projekt jest podzielony na kilka warstw (tutaj folderów) w celu wyraźnego oddzielenia modelu danych od logiki biznesowej i interfejsu uruchomieniowego. 

* **Domain (Modele):** Zawiera tylko byty biznesowe (`User`, `EquipmentItem`, `Rental`).
* **Services:** Klasa `RentalService` koordynująca działania.
* **Policies:** Łatwo podmienialne / edytowalne reguły biznesowe (kalkulator kar).
* **Exceptions:** Jawna obsługa błędów za pomocą własnych typów - konkretne wskazania na źródła błędów wraz z wiadomościami.

### Odpowiedzialności klas (Single Responsibility)
Zestaw odpowiedzialności został rozdrobniony pomiędzy klasy. Każda klasa odpowiada tylko za określone zestawy zbliżonych do siebie czynności. Klasy mają pojedyncze odpowiedzialności - zgrupowane według części wspólnych zadań, jakie pełnią:

- Klasy domenowe:
--> `EquipmentItem` i jego podtypy (`Laptop`, `Camera`) przechowują tylko własny stan wyposażenia.
--> `User` i jego podtypy (`Student`, `Employee`) przechowują tylko własne dane użytkowników z limitami wypożyczeń.
--> `Rental` przetrzymuje informacje o zapisie wypożyczenia (które dane mogą być uzupełniane po jego wydaniu) wraz z odniesieniem do użytkownika czy określonego wyposażenia.

- Klasy zasad / polityk:
--> `StandardPenaltyCalculator` zajmuje się wyłącznie matematycznym wyliczaniem kary.

- Klasy serwisowe:
--> `RentalService` spina powyższe w całość – to on sprawdza, czy limity zostały przekroczone lub czy sprzęt jest dostępny.

- Interfejs użytkownika:
--> `Program.cs` pełni jedynie rolę prezentacyjną (uruchomieniową).

### Kohezja (Cohesion) i Hermetyzacja
Klasy domenowe same zarządzają swoim stanem, nie pozwalając na niekontrolowane modyfikacje z zewnątrz. Przykładem jest właściwość `IsAvailable` w klasie `EquipmentItem`. Posiada ona modyfikator `private set`, a jej zmiana jest możliwa wyłącznie poprzez metody `MarkAsUnavailable()` i `MarkAsAvailable()`. Dzięki temu klasa jest spójna i zawsze gwarantuje poprawność własnych danych. Podobny mechanizm zabezpiecza datę zwrotu i kwotę kary w klasie `Rental`.

### Zmniejszenie sprzężenia (Low Coupling)
Chcąc uniknąć sytuacji, w której zmiana reguł naliczania kar wymusza modyfikację głównego serwisu wypożyczalni, ich logika została wyciągnięta do interfejsu `IPenaltyCalculator`. Klasa `RentalService` przyjmuje ten interfejs poprzez konstruktor (Dependency Injection). Serwis nie wie, jak naliczane są kary – on tylko zleca to zadanie zewnętrznemu kalkulatorowi. Umożliwia to np. bezproblemowe dodanie "darmowych weekendów" poprzez wstrzyknięcie innej implementacji tego interfejsu w przyszłości.

## PODZIAŁ KLAS - Uzasadnienie
- Polimorfizm zamiast instrukcji warunkowych:
Zamiast tworzyć instrukcje `if/switch` sprawdzające typ użytkownika w celu weryfikacji limitów, limity zostały zaszyte bezpośrednio w klasach `Student` i `Employee` poprzez nadpisanie abstrakcyjnej właściwości `MaxConcurrentRentals`. Dzięki temu dodanie nowego typu użytkownika (np. "Gość") nie wymaga modyfikacji kodu sprawdzającego limity w serwisie.
- Jeden plik - jedna klasa:
Za wyjątkiem wyjątków (gdzie może być ich ograniczona ilość a opisy są jednolinijkowe) każda klasa lub interfejs jest reprezentowany przez jeden plik w celu ułatwienia orientacji w projekcie.
- Warstwy projektu w postaci folderów:
Dzięki tej strategii ułatwiono znacznie możliwość grupowania elementów odpowiedzialnych za zbliżone funkcje.