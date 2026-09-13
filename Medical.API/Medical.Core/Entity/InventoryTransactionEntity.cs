namespace Medical.Core.Entity;

public class InventoryTransactionEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MedicineBatchId { get; set; }
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }

    public virtual MedicineBatchEntity? Batch { get; set; }
}
