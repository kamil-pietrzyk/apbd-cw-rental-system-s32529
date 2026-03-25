namespace apbd_cw_rental_system_s32529.Domain.Users;

public class Student(string firstName, string lastName) : User(firstName, lastName)
{
    public override int MaxConcurrentRentals => 2; // Limit dla studenta [cite: 43]
}