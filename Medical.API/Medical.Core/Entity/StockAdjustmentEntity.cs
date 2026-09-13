namespace Medical.Core.Entity;
public class StockAdjustmentEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid MedicineBatchId { get; set; }

    public int PreviousQuantity { get; set; }

    public int AdjustedQuantity { get; set; }

    public string? Reason { get; set; }

    public DateTime AdjustedAt { get; set; }

    public virtual MedicineBatchEntity? Batch { get; set; }
}
