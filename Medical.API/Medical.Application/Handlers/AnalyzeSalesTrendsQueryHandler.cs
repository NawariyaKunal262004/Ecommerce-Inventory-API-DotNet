namespace Medical.Application.Handlers;
public class AnalyzeSalesTrendsQueryHandler : IRequestHandler<AnalyzeSalesTrendsQuery, SalesTrendAnalysisResponse>
{
    private readonly IAIService _aiService;

    public AnalyzeSalesTrendsQueryHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<SalesTrendAnalysisResponse> Handle(AnalyzeSalesTrendsQuery request, CancellationToken cancellationToken)
    {
        return await _aiService.AnalyzeSalesTrendsAsync(request.Days, cancellationToken);
    }
}
