namespace Medical.Application.Queries;
public class AutoCalculateBillTotalQuery : IRequest<BillTotalCalculationResponse>
{
    public List<AddBillItemRequest> Items { get; set; } = new List<AddBillItemRequest>();
    public decimal? Discount { get; set; }
    public decimal? TaxRate { get; set; }
}
