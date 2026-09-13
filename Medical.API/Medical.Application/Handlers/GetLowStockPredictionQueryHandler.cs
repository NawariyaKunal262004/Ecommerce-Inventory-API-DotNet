namespace Medical.Application.Handlers;
public class GetLowStockPredictionQueryHandler : IRequestHandler<GetLowStockPredictionQuery, List<StockPredictionResponse>>
{
    private readonly IAIService _aiService;

    public GetLowStockPredictionQueryHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<List<StockPredictionResponse>> Handle(GetLowStockPredictionQuery request, CancellationToken cancellationToken)
    {
        return await _aiService.GetLowStockPredictionAsync(cancellationToken);
    }
}
