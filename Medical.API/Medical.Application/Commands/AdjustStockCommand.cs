namespace Medical.Application.Commands;
public class AdjustStockCommand : IRequest<ResponseModel>
{
    public Guid MedicineBatchId { get; set; }
    public int AdjustedQuantity { get; set; }
    public string? Reason { get; set; }
}
