namespace Medical.Application.Queries;
public class AnalyzeSalesTrendsQuery : IRequest<SalesTrendAnalysisResponse>
{
    public int Days { get; set; } = 30;
}
