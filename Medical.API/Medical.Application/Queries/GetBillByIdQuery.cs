namespace Medical.Application.Queries;
public class GetBillByIdQuery : IRequest<BillResponse>
{
    public Guid BillId { get; set; }
}
