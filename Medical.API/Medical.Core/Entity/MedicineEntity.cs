namespace Medical.Core.Entity;

public class MedicineEntity
{
    [Key]
    public Guid MedicineId { get; set; } = Guid.NewGuid();

    public required string MedicineName { get; set; } = string.Empty;

    public string? MedicineCategory { get; set; } = null;

    public decimal MedicinePrice { get; set; }

    public int Stock { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public Guid SupplierId { get; set; }

    public DateOnly ManufacturingDate { get; set; }

    public string? Manufacturer { get; set; } = null;

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public virtual SupplierEntity? Supplier { get; set; }

    public virtual ICollection<MedicineBatchEntity> Batches { get; set; } = new List<MedicineBatchEntity>();

    public virtual ICollection<BillItemEntity> BillItems { get; set; } = new List<BillItemEntity>();
}
