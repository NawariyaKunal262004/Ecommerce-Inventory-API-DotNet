namespace Medical.Infra.Repositories;
public class MedicineBatchRepository : IMedicineBatchRepository
{
    private readonly ApplicationDbContext _context;
    public MedicineBatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MedicineBatchEntity?> GetByIdAsync(Guid batchId)
    {
        return await _context.MedicineBatches
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == batchId);
    }

    public async Task<IReadOnlyList<MedicineBatchEntity>> GetBatchesByMedicineIdAsync(Guid medicineId)
    {
        return await _context.MedicineBatches
            .AsNoTracking()
            .Where(b => b.MedicineId == medicineId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicineBatchEntity>> GetAvailableBatchesAsync(Guid medicineId, DateTime asOfDate)
    {
        return await _context.MedicineBatches
            .AsNoTracking()
            .Where(b => b.MedicineId == medicineId && b.ExpiryDate > asOfDate && b.QuantityAvailable > 0)
            .OrderBy(b => b.ManufacturingDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicineBatchEntity>> GetBatchesForStockRestoreAsync(Guid medicineId, DateTime asOfDate)
    {
        return await _context.MedicineBatches
            .AsNoTracking()
            .Where(b => b.MedicineId == medicineId && b.ExpiryDate > asOfDate)
            .OrderBy(b => b.ManufacturingDate)
            .ToListAsync();
    }

    public async Task AddAsync(MedicineBatchEntity batch)
    {
        _context.MedicineBatches.Add(batch);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MedicineBatchEntity batch)
    {
        await _context.MedicineBatches
            .Where(b => b.Id == batch.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(b => b.BatchNumber, batch.BatchNumber)
                .SetProperty(b => b.QuantityAvailable, batch.QuantityAvailable)
                .SetProperty(b => b.PurchasePrice, batch.PurchasePrice)
                .SetProperty(b => b.SellingPrice, batch.SellingPrice)
                .SetProperty(b => b.ExpiryDate, batch.ExpiryDate)
                .SetProperty(b => b.SupplierId, batch.SupplierId));
    }

    public async Task DeleteAsync(Guid batchId)
    {
        var batch = await _context.MedicineBatches.FindAsync(batchId);
        if (batch != null)
        {
            _context.MedicineBatches.Remove(batch);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IReadOnlyList<MedicineBatchEntity>> GetExpiringBatchesAsync(DateTime expiryThreshold)
    {
        return await _context.MedicineBatches.Where(b => b.ExpiryDate <= expiryThreshold && b.QuantityAvailable > 0).ToListAsync();
    }

    public async Task<IReadOnlyList<MedicineBatchEntity>> GetExpiredBatchesAsync(DateTime asOfDate)
    {
        return await _context.MedicineBatches.Where(b => b.ExpiryDate < asOfDate && b.QuantityAvailable > 0).ToListAsync();
    }
}
