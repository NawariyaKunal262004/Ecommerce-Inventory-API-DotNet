namespace Medical.Application.Responses;
public class MedicineSuggestionResponse
{
    public Guid MedicineId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string? MedicineCategory { get; set; }
    public decimal MedicinePrice { get; set; }
    public int Stock { get; set; }
    public decimal ConfidenceScore { get; set; }
    public string Reason { get; set; } = string.Empty;
}
