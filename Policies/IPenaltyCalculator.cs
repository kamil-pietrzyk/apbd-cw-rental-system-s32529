namespace apbd_cw_rental_system_s32529.Policies;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(DateTime dueDate, DateTime returnDate);
}

// Przykładowa implementacja: stała kwota za każdy dzień zwłoki
public class StandardPenaltyCalculator(decimal dailyRate) : IPenaltyCalculator
{
    public decimal CalculatePenalty(DateTime dueDate, DateTime returnDate)
    {
        if (returnDate <= dueDate) return 0m;
        
        int daysLate = (returnDate - dueDate).Days;
        return daysLate * dailyRate;
    }
}