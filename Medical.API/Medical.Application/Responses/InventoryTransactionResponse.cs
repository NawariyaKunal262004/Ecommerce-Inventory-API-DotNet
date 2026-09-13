namespace Medical.Application.Responses;
public class InventoryTransactionResponse
{
    public Guid Id { get; set; }
    public Guid MedicineBatchId { get; set; }
    public Guid MedicineId { get; set; }
    public string? MedicineName { get; set; }
    public int Quantity { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }
}
