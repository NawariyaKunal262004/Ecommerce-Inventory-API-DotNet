namespace Medical.Application.Commands;
public class AddMedicineBatchCommand : IRequest<Guid>
{
    public Guid MedicineId { get; set; }

    public string? BatchNumber { get; set; }

    public int QuantityAvailable { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public DateTime ExpiryDate { get; set; }

    public Guid SupplierId { get; set; }
}
