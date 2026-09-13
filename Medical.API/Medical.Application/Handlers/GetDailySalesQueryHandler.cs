namespace Medical.Application.Handlers;
public class GetDailySalesQueryHandler : IRequestHandler<GetDailySalesQuery, decimal>
{
    private readonly IBillRepository _billRepository;

    public GetDailySalesQueryHandler(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<decimal> Handle(GetDailySalesQuery request, CancellationToken cancellationToken)
    {
        return await _billRepository.GetDailySalesAsync(request.Date);
    }
}
