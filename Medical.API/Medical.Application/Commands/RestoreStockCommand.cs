namespace Medical.Application.Commands;
public class RestoreStockCommand : IRequest<ResponseModel>
{
    public Guid MedicineBatchId { get; set; }
    public int Quantity { get; set; }
    public string? Reason { get; set; }
}
