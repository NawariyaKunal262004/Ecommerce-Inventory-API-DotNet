namespace Medical.Application.Events;
public class MedicineExpiredEvent
{
    public Guid MedicineBatchId { get; set; }
    public Guid MedicineId { get; set; }
    public DateTime ExpiryDate { get; set; }
}
