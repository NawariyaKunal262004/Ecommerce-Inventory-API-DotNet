namespace Medical.Application.Responses;
public class InventorySummaryResponse
{
    public int TotalStock { get; set; }
    public int LowStockCount { get; set; }
    public int ExpiredStockCount { get; set; }
}
