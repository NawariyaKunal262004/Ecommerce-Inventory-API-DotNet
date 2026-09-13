namespace Medical.Application.Events;
public class StockDeductedEvent
{
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
    public DateTime DeductedAt { get; set; }
}
