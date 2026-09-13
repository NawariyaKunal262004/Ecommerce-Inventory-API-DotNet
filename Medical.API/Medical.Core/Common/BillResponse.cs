namespace Medical.Core.Common;
public class BillResponse
{
    public Guid BillId { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal FinalAmount { get; set; }
    public List<BillItemResponse> Items { get; set; } = new List<BillItemResponse>();
}

public class BillItemResponse
{
    public Guid BillItemId { get; set; }
    public Guid MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
