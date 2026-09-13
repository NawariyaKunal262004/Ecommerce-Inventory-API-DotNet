namespace Medical.Core.Entity;

public class BillEntity
{
    public Guid BillId { get; set; } = Guid.NewGuid();

    public Guid PatientId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal Discount { get; set; } = 0;

    public decimal Tax { get; set; } = 0;

    public decimal FinalAmount { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<BillItemEntity> BillItems { get; set; } = new List<BillItemEntity>();

    public virtual PatientEntity? Patient { get; set; }

    public void RecalculateTotals(decimal taxRate)
    {
        TotalAmount = BillItems.Sum(bi => bi.TotalPrice);
        Tax = TotalAmount * taxRate;
        FinalAmount = TotalAmount + Tax - Discount;
    }
}
