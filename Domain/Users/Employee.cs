namespace apbd_cw_rental_system_s32529.Domain.Users;

public class Employee(string firstName, string lastName) : User(firstName, lastName)
{
    public override int MaxConcurrentRentals => 5; // Limit dla pracownika
}