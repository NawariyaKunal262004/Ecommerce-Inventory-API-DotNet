namespace Medical.Application.Handlers;
public class AutoCalculateBillTotalQueryHandler : IRequestHandler<AutoCalculateBillTotalQuery, BillTotalCalculationResponse>
{
    private readonly IAIService _aiService;

    public AutoCalculateBillTotalQueryHandler(IAIService aiService)
    {
        _aiService = aiService;
    }

    public async Task<BillTotalCalculationResponse> Handle(AutoCalculateBillTotalQuery request, CancellationToken cancellationToken)
    {
        return await _aiService.AutoCalculateBillTotalAsync(request.Items, request.Discount, request.TaxRate, cancellationToken);
    }
}
