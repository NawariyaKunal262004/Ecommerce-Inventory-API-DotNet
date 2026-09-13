namespace Medical.Application.Commands;
public class DeleteMedicineBatchCommand : IRequest<ResponseModel>
{
    public Guid Id { get; set; }
}
