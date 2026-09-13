namespace Medical.Infra.Repositories;
public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    private readonly ApplicationDbContext _context;
    public InventoryTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(InventoryTransactionEntity transaction)
    {
        _context.InventoryTransactions.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<InventoryTransactionEntity>> GetByMedicineIdAsync(Guid medicineId)
    {
        return await _context.InventoryTransactions.Where(t => t.MedicineId == medicineId).ToListAsync();
    }

    public async Task<IReadOnlyList<InventoryTransactionEntity>> GetAllAsync()
    {
        return await _context.InventoryTransactions.ToListAsync();
    }
}
