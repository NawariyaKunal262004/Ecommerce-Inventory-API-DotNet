namespace Medical.Infra.Repositories;

public class BillRepository : RepositoryBase<BillEntity>, IBillRepository
{
    public BillRepository(ApplicationDbContext context, ILogger<RepositoryBase<BillEntity>> logger) : base(context, logger) { }

    public async Task<BillEntity> CreateBillAsync(BillEntity bill) => await AddAsync(bill);

    public async Task<BillEntity> AddBillItemAsync(Guid billId, BillItemEntity billItem)
    {
        _context.BillItems.Add(billItem);
        await _context.SaveChangesAsync();

        return await GetBillByIdAsync(billId)
            ?? throw new InvalidOperationException($"Bill with ID {billId} not found.");
    }

    public async Task<BillEntity?> GetBillByIdAsync(Guid billId)
    {
        return await _context.Bills
            .AsNoTracking()
            .Include(b => b.BillItems)
            .ThenInclude(bi => bi.Medicine)
            .Include(b => b.Patient)
            .FirstOrDefaultAsync(b => b.BillId == billId);
    }

    public override async Task<BillEntity> UpdateAsync(BillEntity bill) => await base.UpdateAsync(bill);

    public async Task<decimal> GetDailySalesAsync(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

        return await _context.Bills
            .Where(b => b.CreatedDate >= startOfDay && b.CreatedDate <= endOfDay)
            .SumAsync(b => b.TotalAmount);
    }

    public async Task<IReadOnlyList<BillEntity>> GetAllBillsWithItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bills
            .Include(b => b.BillItems)
            .ThenInclude(bi => bi.Medicine)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<BillItemEntity?> GetBillItemByIdAsync(Guid billItemId)
    {
        return await _context.BillItems
            .Include(bi => bi.Bill)
            .Include(bi => bi.Medicine)
            .AsNoTracking()
            .FirstOrDefaultAsync(bi => bi.BillItemId == billItemId);
    }

    public async Task DeleteBillAsync(Guid billId)
    {
        await _context.BillItems
            .Where(bi => bi.BillId == billId)
            .ExecuteDeleteAsync();

        await _context.Bills
            .Where(b => b.BillId == billId)
            .ExecuteDeleteAsync();
    }

    public Task DeleteBillItemAsync(Guid billItemId) =>
        _context.BillItems
            .Where(bi => bi.BillItemId == billItemId)
            .ExecuteDeleteAsync();
}
