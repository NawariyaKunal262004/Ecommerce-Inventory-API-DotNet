namespace Medical.Application.Commands;
public class CreateInventoryTransactionCommand : IRequest<Guid>
{
    public Guid MedicineBatchId { get; set; }
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }
}
