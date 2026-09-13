namespace Medical.Application.Responses;
public class SalesTrendAnalysisResponse
{
    public decimal TotalRevenueLast7Days { get; set; }
    public decimal TotalRevenueLast30Days { get; set; }
    public decimal RevenueGrowthPercentage { get; set; }
    public List<DailySalesItem> DailySales { get; set; } = new List<DailySalesItem>();
    public List<TopSellingMedicine> TopSellingMedicines { get; set; } = new List<TopSellingMedicine>();
}

public class DailySalesItem
{
    public DateTime Date { get; set; }
    public decimal TotalSales { get; set; }
}

public class TopSellingMedicine
{
    public Guid MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
}
