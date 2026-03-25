using System;
using System.Collections.Generic;
using System.Linq;
using apbd_cw_rental_system_s32529.Domain.Equipment;
using apbd_cw_rental_system_s32529.Domain.Rentals;
using apbd_cw_rental_system_s32529.Domain.Users;
using apbd_cw_rental_system_s32529.Exceptions;
using apbd_cw_rental_system_s32529.Policies;

namespace apbd_cw_rental_system_s32529.Services;

public class RentalService(IPenaltyCalculator penaltyCalculator)
{
    private readonly List<User> _users = [];
    private readonly List<EquipmentItem> _equipment = [];
    private readonly List<Rental> _rentals = [];

    public void AddUser(User user) => _users.Add(user);
    public void AddEquipment(EquipmentItem item) => _equipment.Add(item);

    public Rental RentEquipment(Guid userId, Guid equipmentId, TimeSpan duration)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId) 
                   ?? throw new DomainException("User not found.");
        
        var equipment = _equipment.FirstOrDefault(e => e.Id == equipmentId) 
                        ?? throw new EquipmentNotFoundException("Equipment not found.");

        // Reguła: niedostępnego sprzętu nie wypożyczamy 
        if (!equipment.IsAvailable)
            throw new EquipmentUnavailableException($"Equipment {equipment.Name} is currently unavailable.");

        // Reguła: weryfikacja limitów użytkownika 
        int activeRentalsCount = _rentals.Count(r => r.Borrower.Id == user.Id && r.IsActive);
        if (activeRentalsCount >= user.MaxConcurrentRentals)
            throw new UserLimitExceededException($"{user.FirstName} has reached the maximum of {user.MaxConcurrentRentals} rentals.");

        // Tworzymy wypożyczenie i oznaczamy sprzęt
        var rental = new Rental(user, equipment, DateTime.Now, duration);
        equipment.MarkAsUnavailable();
        _rentals.Add(rental);

        return rental;
    }

    public void ReturnEquipment(Guid rentalId)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId) 
                     ?? throw new DomainException("Rental not found.");

        if (!rental.IsActive)
            throw new DomainException("This rental is already closed.");

        var returnDate = DateTime.Now;
        var penalty = penaltyCalculator.CalculatePenalty(rental.DueDate, returnDate);

        rental.CompleteReturn(returnDate, penalty);
        rental.Item.MarkAsAvailable();
    }
    
    // Oznaczenie sprzętu jako niedostępnego (np. uszkodzenie)
    public void DisableEquipment(Guid equipmentId)
    {
        var equipment = _equipment.FirstOrDefault(e => e.Id == equipmentId) 
                        ?? throw new EquipmentNotFoundException("Equipment not found.");
        equipment.MarkAsUnavailable();
    }
    
    public IEnumerable<EquipmentItem> GetAllEquipment() => _equipment;
    
    public IEnumerable<EquipmentItem> GetAvailableEquipment() => _equipment.Where(e => e.IsAvailable);
    
    public IEnumerable<Rental> GetActiveRentalsForUser(Guid userId) 
        => _rentals.Where(r => r.Borrower.Id == userId && r.IsActive);
    
    public IEnumerable<Rental> GetOverdueRentals() 
        => _rentals.Where(r => r.IsOverdue);

    // Pobranie wszystkich użytkowników (przydatne do UI)
    public IEnumerable<User> GetAllUsers() => _users;
    
    public (int TotalEquipment, int AvailableEquipment, int ActiveRentals, int OverdueRentals) GenerateSummaryReport()
    {
        return (
            TotalEquipment: _equipment.Count,
            AvailableEquipment: _equipment.Count(e => e.IsAvailable),
            ActiveRentals: _rentals.Count(r => r.IsActive),
            OverdueRentals: _rentals.Count(r => r.IsOverdue)
        );
    }
}