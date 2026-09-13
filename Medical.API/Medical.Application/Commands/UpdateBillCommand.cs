namespace Medical.Application.Commands;
public class UpdateBillCommand : IRequest<BillResponse>
{
    public Guid BillId { get; set; }
    public decimal? Discount { get; set; }
}
