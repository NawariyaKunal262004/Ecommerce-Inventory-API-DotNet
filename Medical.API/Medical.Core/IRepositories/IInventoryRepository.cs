namespace Medical.Core.IRepositories;
public interface IInventoryRepository
{
    Task<int> GetTotalStockAsync();
    Task<int> GetLowStockCountAsync(int threshold);
    Task<int> GetExpiredStockCountAsync(DateTime asOfDate);
    Task<IReadOnlyList<InventoryTransactionEntity>> GetInventoryTransactionsAsync(Guid? medicineId = null);
}
