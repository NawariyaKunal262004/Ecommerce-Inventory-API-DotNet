namespace Medical.Application.Handlers;
public class GetInventorySummaryQueryHandler : IRequestHandler<GetInventorySummaryQuery, InventorySummaryResponse>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetInventorySummaryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<InventorySummaryResponse> Handle(GetInventorySummaryQuery request, CancellationToken cancellationToken)
    {
        var totalStock = await _inventoryRepository.GetTotalStockAsync();
        var lowStockCount = await _inventoryRepository.GetLowStockCountAsync(10);
        var expiredStockCount = await _inventoryRepository.GetExpiredStockCountAsync(DateTime.UtcNow);

        return new InventorySummaryResponse
        {
            TotalStock = totalStock,
            LowStockCount = lowStockCount,
            ExpiredStockCount = expiredStockCount
        };
    }
}
