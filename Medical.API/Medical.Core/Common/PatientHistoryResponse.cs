namespace Medical.Core.Common;
public class PatientHistoryResponse
{
    public Guid PatientId { get; set; }

    public string? PatientName { get; set; }

    public int Age { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int TotalBills { get; set; }

    public decimal TotalAmountSpent { get; set; }

    public List<PatientBillHistoryItem> Bills { get; set; } = new List<PatientBillHistoryItem>();
}

public class PatientBillHistoryItem
{
    public Guid BillId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal FinalAmount { get; set; }

    public DateTime CreatedDate { get; set; }

    public int ItemCount { get; set; }

    public List<PatientBillItemDetail> Items { get; set; } = new List<PatientBillItemDetail>();
}

public class PatientBillItemDetail
{
    public string? MedicineName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }
}
