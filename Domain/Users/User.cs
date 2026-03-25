namespace apbd_cw_rental_system_s32529.Domain.Users;

public abstract class User(string firstName, string lastName)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;

    // Polimorfizm - typy pochodne same określają swój limit
    public abstract int MaxConcurrentRentals { get; }
}