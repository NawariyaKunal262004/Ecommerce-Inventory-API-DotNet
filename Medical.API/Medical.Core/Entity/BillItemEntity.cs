namespace Medical.Core.Entity;

public class BillItemEntity
{
    public Guid BillItemId { get; set; } = Guid.NewGuid();
    public Guid BillId { get; set; }
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public virtual BillEntity? Bill { get; set; }
    public virtual MedicineEntity? Medicine { get; set; }
}
