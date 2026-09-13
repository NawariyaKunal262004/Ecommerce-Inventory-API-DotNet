namespace Medical.Application.Commands;
public class CreateMedicineCommand : IRequest<ResponseModel>
{
    
    public string? MedicineName { get; set; }
    public string? MedicineCategory { get; set; }
    public decimal MedicinePrice { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public Guid SupplierId { get; set; }
    public DateOnly ManufacturingDate { get; set; }
    public string? Manufacturer { get; set; } = null;
}
