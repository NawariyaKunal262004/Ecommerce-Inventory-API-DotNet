namespace Medical.Core.Common;
public class MedicineResponse
{
    public Guid MedicineId { get; set; }
    public string? MedicineName { get; set; }
    public string? MedicineCategory { get; set; }
    public decimal MedicinePrice { get; set; }

    public int Stock { get; set; }

    public DateOnly ExpirationDate { get; set; }
    public Guid SupplierID { get; set; }
    public DateOnly ManufacturingDate { get; set; }
    public string? Manufacturer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
