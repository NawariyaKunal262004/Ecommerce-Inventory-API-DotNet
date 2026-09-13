namespace Medical.Application.Commands;
public class DeleteBillItemCommand : IRequest<BillResponse>
{
    public Guid BillItemId { get; set; }
}
