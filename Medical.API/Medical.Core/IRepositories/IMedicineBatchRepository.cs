namespace Medical.Core.IRepositories;
public interface IMedicineBatchRepository
{
    Task<MedicineBatchEntity?> GetByIdAsync(Guid batchId);
    Task<IReadOnlyList<MedicineBatchEntity>> GetBatchesByMedicineIdAsync(Guid medicineId);
    Task<IReadOnlyList<MedicineBatchEntity>> GetAvailableBatchesAsync(Guid medicineId, DateTime asOfDate);

    Task<IReadOnlyList<MedicineBatchEntity>> GetBatchesForStockRestoreAsync(Guid medicineId, DateTime asOfDate);
    Task AddAsync(MedicineBatchEntity batch);
    Task UpdateAsync(MedicineBatchEntity batch);
    Task DeleteAsync(Guid batchId);
    Task<IReadOnlyList<MedicineBatchEntity>> GetExpiringBatchesAsync(DateTime expiryThreshold);
    Task<IReadOnlyList<MedicineBatchEntity>> GetExpiredBatchesAsync(DateTime asOfDate);
}
