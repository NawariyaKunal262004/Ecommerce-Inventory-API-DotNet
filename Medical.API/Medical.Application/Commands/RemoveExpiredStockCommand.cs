namespace Medical.Application.Commands;
public class RemoveExpiredStockCommand : IRequest<ResponseModel>
{
    public Guid MedicineBatchId { get; set; }
}
