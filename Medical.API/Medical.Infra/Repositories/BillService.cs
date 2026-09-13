namespace Medical.Infra.Repositories;
public class BillService : IBillService
{
    private readonly ApplicationDbContext _context;

    public BillService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RecalculateAndPersistTotalsAsync(Guid billId, decimal taxRate, CancellationToken cancellationToken = default)
    {
        var bill = await _context.Bills
            .AsNoTracking()
            .Include(b => b.BillItems)
            .FirstOrDefaultAsync(b => b.BillId == billId, cancellationToken);

        if (bill == null)
            throw new InvalidOperationException($"Bill with ID {billId} not found.");

        bill.RecalculateTotals(taxRate);

        await _context.Bills
            .Where(b => b.BillId == billId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(b => b.TotalAmount, bill.TotalAmount)
                    .SetProperty(b => b.Tax, bill.Tax)
                    .SetProperty(b => b.FinalAmount, bill.FinalAmount),
                cancellationToken);
    }

    public Task UpdateDiscountAsync(Guid billId, decimal discount, CancellationToken cancellationToken) =>
        _context.Bills
            .Where(b => b.BillId == billId)
            .ExecuteUpdateAsync(s => s.SetProperty(b => b.Discount, discount), cancellationToken);

    public async Task AddBillItemAsync(BillItemEntity billItem, CancellationToken cancellationToken = default)
    {
        _context.BillItems.Add(billItem);
        await Task.CompletedTask;
    }

    public Task UpdateBillItemAsync(Guid billItemId, int quantity, decimal totalPrice, CancellationToken cancellationToken) =>
        _context.BillItems
            .Where(bi => bi.BillItemId == billItemId)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(bi => bi.Quantity, quantity)
                    .SetProperty(bi => bi.TotalPrice, totalPrice),
                cancellationToken);

    public Task DeleteBillItemAsync(Guid billItemId, CancellationToken cancellationToken) =>
        _context.BillItems
            .Where(bi => bi.BillItemId == billItemId)
            .ExecuteDeleteAsync(cancellationToken);

    public async Task DeleteBillAsync(Guid billId, CancellationToken cancellationToken = default)
    {
        await _context.BillItems
            .Where(bi => bi.BillId == billId)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.Bills
            .Where(b => b.BillId == billId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
