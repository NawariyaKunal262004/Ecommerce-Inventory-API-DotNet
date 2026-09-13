namespace Medical.Application.Commands;
public class DeleteBillCommand : IRequest<bool>
{
    public Guid BillId { get; set; }
}
