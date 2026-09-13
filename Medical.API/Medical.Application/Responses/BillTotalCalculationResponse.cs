namespace Medical.Application.Responses;
public class BillTotalCalculationResponse
{
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal FinalTotal { get; set; }
    public List<string> AppliedDiscounts { get; set; } = new List<string>();
    public List<string> Warnings { get; set; } = new List<string>();
}
