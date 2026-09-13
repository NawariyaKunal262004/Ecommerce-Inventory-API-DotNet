namespace Medical.Application.Responses;
public class MedicineBatchResponse
{
    public Guid Id { get; set; }
    public Guid MedicineId { get; set; }
    public string? MedicineName { get; set; }
    public string? BatchNumber { get; set; }
    public int QuantityAvailable { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public DateTime ManufacturingDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public Guid SupplierId { get; set; }
    public bool IsExpired { get; set; }
}
