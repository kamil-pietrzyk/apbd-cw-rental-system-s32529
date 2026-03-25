namespace apbd_cw_rental_system_s32529.Domain.Equipment;

public abstract class EquipmentItem(string name)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; } = name;
    public bool IsAvailable { get; private set; } = true;
    
    public void MarkAsUnavailable() => IsAvailable = false;
    public void MarkAsAvailable() => IsAvailable = true;
}