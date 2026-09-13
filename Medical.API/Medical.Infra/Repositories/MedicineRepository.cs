namespace Medical.Infra.Repositories;
public class MedicineRepository : RepositoryBase<MedicineEntity>, IMedicineRepository
{
    public MedicineRepository(ApplicationDbContext context, ILogger<RepositoryBase<MedicineEntity>> logger) : base(context, logger)
    {
    }

    public async Task<IReadOnlyList<MedicineEntity>> GetLowStockAsync()
    {
        return await _context.Medicines
            .Where(m => m.Stock < 10)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicineEntity>> GetExpiringAsync()
    {
        // Calculate the date 30 days from now
        var expiryThreshold = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
        
        return await _context.Medicines
            .Where(m => m.ExpirationDate <= expiryThreshold && m.ExpirationDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync();
    }
}
