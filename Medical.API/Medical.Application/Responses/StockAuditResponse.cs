namespace Medical.Application.Responses;
public class StockAuditResponse
{
    public Guid Id { get; set; }
    public Guid MedicineBatchId { get; set; }
    public string? MedicineName { get; set; }
    public string? BatchNumber { get; set; }
    public int PreviousQuantity { get; set; }
    public int AdjustedQuantity { get; set; }
    public int CurrentQuantity { get; set; }
    public string? Reason { get; set; }
    public DateTime AdjustedAt { get; set; }
}
