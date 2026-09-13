namespace Medical.Application.Queries;
public class GetMedicineBatchByIdQuery : IRequest<MedicineBatchResponse>
{
    public Guid BatchId { get; set; }
}
