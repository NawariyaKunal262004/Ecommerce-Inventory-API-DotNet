namespace Medical.Application.Events;
public class LowStockDetectedEvent
{
    public Guid MedicineId { get; set; }
    public int AvailableQuantity { get; set; }
    public int Threshold { get; set; }
    public DateTime DetectedAt { get; set; }
}
