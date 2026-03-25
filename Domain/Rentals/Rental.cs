using apbd_cw_rental_system_s32529.Domain.Users;
using apbd_cw_rental_system_s32529.Domain.Equipment;

namespace apbd_cw_rental_system_s32529.Domain.Rentals;

public class Rental(User borrower, EquipmentItem item, DateTime rentDate, TimeSpan duration)
{
    public Guid Id { get; } = Guid.NewGuid();
    public User Borrower { get; } = borrower;
    public EquipmentItem Item { get; } = item;
    public DateTime RentDate { get; } = rentDate;
    public DateTime DueDate { get; } = rentDate.Add(duration);
    public DateTime? ReturnDate { get; private set; }
    public decimal PenaltyFee { get; private set; }
    
    public bool IsActive => ReturnDate == null;
    public bool IsOverdue => IsActive && DateTime.Now > DueDate;

    public void CompleteReturn(DateTime returnDate, decimal penaltyFee)
    {
        ReturnDate = returnDate;
        PenaltyFee = penaltyFee;
    }
}