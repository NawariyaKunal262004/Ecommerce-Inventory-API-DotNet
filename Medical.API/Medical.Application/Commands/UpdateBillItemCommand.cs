namespace Medical.Application.Commands;
public class UpdateBillItemCommand : IRequest<BillResponse>
{
    public Guid BillItemId { get; set; }
    public int NewQuantity { get; set; }
}
