namespace Medical.Application.Responses;
public class StockPredictionResponse
{
    public Guid MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public decimal AverageDailyUsage { get; set; }
    public int EstimatedDaysRemaining { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public string Suggestion { get; set; } = string.Empty;
}
