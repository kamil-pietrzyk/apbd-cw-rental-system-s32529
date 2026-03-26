namespace apbd_cw_rental_system_s32529.Policies;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(DateTime dueDate, DateTime returnDate);
}