namespace Medical.Application.Handlers;
public class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand, bool>
{
    private readonly IBillRepository _billRepository;
    private readonly IBillInventoryService _billInventoryService;
    private readonly IBillService _billService;
    private readonly ApplicationDbContext _context;

    public DeleteBillCommandHandler(
        IBillRepository billRepository,
        IBillInventoryService billInventoryService,
        IBillService billService,
        ApplicationDbContext context)
    {
        _billRepository = billRepository;
        _billInventoryService = billInventoryService;
        _billService = billService;
        _context = context;
    }

    public async Task<bool> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var bill = await _billRepository.GetBillByIdAsync(request.BillId);
            if (bill == null)
                throw new BillNotFoundException($"Bill with ID {request.BillId} not found.");

            var stockByMedicine = bill.BillItems
                .GroupBy(bi => bi.MedicineId)
                .Select(g => new { MedicineId = g.Key, Quantity = g.Sum(bi => bi.Quantity) });

            foreach (var item in stockByMedicine)
            {
                await _billInventoryService.RestoreStockAsync(
                    item.MedicineId, item.Quantity, bill.BillId, cancellationToken);
            }

            await _billService.DeleteBillAsync(request.BillId, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
