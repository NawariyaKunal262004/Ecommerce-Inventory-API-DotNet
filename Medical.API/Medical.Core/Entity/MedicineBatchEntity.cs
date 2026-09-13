namespace Medical.Core.Entity;

public class MedicineBatchEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid MedicineId { get; set; }

    public string? BatchNumber { get; set; }

    public int QuantityAvailable { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public DateTime ManufacturingDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public Guid SupplierId { get; set; }

    public virtual MedicineEntity? Medicine { get; set; }

    public virtual ICollection<InventoryTransactionEntity> Transactions { get; set; } = new List<InventoryTransactionEntity>();

    public virtual ICollection<StockAdjustmentEntity> StockAdjustments { get; set; } = new List<StockAdjustmentEntity>();

    public virtual SupplierEntity? Supplier { get; set; }
}
