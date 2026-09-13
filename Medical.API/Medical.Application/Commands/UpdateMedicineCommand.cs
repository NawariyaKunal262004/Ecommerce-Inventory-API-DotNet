namespace Medical.Application.Commands;
public class UpdateMedicineCommand : IRequest<ResponseModel>
{
    public Guid MedicineId { get; set; }
    public string? MedicineName { get; set; }
    public string? MedicineCategory { get; set; }
    public decimal MedicinePrice { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public Guid SupplierID { get; set; }
    public DateOnly ManufacturingDate { get; set; }
    public string? Manufacturer { get; set; } = null;
}
