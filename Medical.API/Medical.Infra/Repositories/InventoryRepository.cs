namespace Medical.Infra.Repositories;
public class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _context;
    public InventoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalStockAsync()
    {
        return await _context.Medicines.SumAsync(m => m.Stock);
    }

    public async Task<int> GetLowStockCountAsync(int threshold)
    {
        return await _context.Medicines.CountAsync(m => m.Stock < threshold);
    }

    public async Task<int> GetExpiredStockCountAsync(DateTime asOfDate)
    {
        return await _context.MedicineBatches.CountAsync(b => b.ExpiryDate < asOfDate);
    }

    public async Task<IReadOnlyList<InventoryTransactionEntity>> GetInventoryTransactionsAsync(Guid? medicineId = null)
    {
        var query = _context.InventoryTransactions.AsQueryable();
        if (medicineId.HasValue)
            query = query.Where(t => t.MedicineId == medicineId.Value);
        return await query.ToListAsync();
    }
}
