namespace Medical.Infra.Repositories;
public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateMedicineTotalStockAsync(
        Guid medicineId,
        bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        var totalStock = await _context.MedicineBatches
            .Where(b => b.MedicineId == medicineId)
            .SumAsync(b => b.QuantityAvailable, cancellationToken);

        await _context.Medicines
            .Where(m => m.MedicineId == medicineId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(m => m.Stock, totalStock)
                    .SetProperty(m => m.LastUpdatedAt, DateTime.UtcNow),
                cancellationToken);

        if (saveChanges)
            await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsStockAvailableAsync(Guid medicineId, int quantity)
    {
        var totalAvailable = await _context.MedicineBatches
            .Where(b => b.MedicineId == medicineId && b.ExpiryDate > DateTime.UtcNow)
            .SumAsync(b => b.QuantityAvailable);

        return totalAvailable >= quantity;
    }
}
