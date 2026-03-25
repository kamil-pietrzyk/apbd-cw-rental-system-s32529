namespace apbd_cw_rental_system_s32529.Domain.Equipment;

public class Laptop(string name, int ramSizeGb, string processorModel) : EquipmentItem(name)
{
    public int RamSizeGb { get; } = ramSizeGb;
    public string ProcessorModel { get; } = processorModel;
}