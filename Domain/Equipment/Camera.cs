namespace apbd_cw_rental_system_s32529.Domain.Equipment;

public class Camera(string name, bool isDigital, string lensType) : EquipmentItem(name)
{
    public bool IsDigital { get; } = isDigital;
    public string LensType { get; } = lensType;
}