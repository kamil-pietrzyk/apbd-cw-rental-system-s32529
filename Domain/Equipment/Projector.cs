namespace apbd_cw_rental_system_s32529.Domain.Equipment;

public class Projector(string name, int lumens, string resolution) : EquipmentItem(name)
{
    public int Lumens { get; } = lumens;
    public string Resolution { get; } = resolution;
}