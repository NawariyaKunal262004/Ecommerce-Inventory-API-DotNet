namespace Medical.Application.Handlers;
public class DeleteBillItemCommandHandler : IRequestHandler<DeleteBillItemCommand, BillResponse>
{
    private readonly IBillRepository _billRepository;
    private readonly IBillInventoryService _billInventoryService;
    private readonly IBillService _billService;
    private readonly IOptions<BillingSettings> _billingSettings;
    private readonly ApplicationDbContext _context;

    public DeleteBillItemCommandHandler(
        IBillRepository billRepository,
        IBillInventoryService billInventoryService,
        IBillService billService,
        IOptions<BillingSettings> billingSettings,
        ApplicationDbContext context)
    {
        _billRepository = billRepository;
        _billInventoryService = billInventoryService;
        _billService = billService;
        _billingSettings = billingSettings;
        _context = context;
    }

    public async Task<BillResponse> Handle(DeleteBillItemCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var billItem = await _billRepository.GetBillItemByIdAsync(request.BillItemId);
            if (billItem == null)
                throw new BillItemNotFoundException($"Bill item with ID {request.BillItemId} not found.");

            if (billItem.Bill == null)
                throw new BillNotFoundException($"Bill not found for bill item {request.BillItemId}.");

            var billId = billItem.BillId;

            await _billInventoryService.RestoreStockAsync(
                billItem.MedicineId, billItem.Quantity, billId, cancellationToken);

            await _billService.DeleteBillItemAsync(request.BillItemId, cancellationToken);
            await _billService.RecalculateAndPersistTotalsAsync(
                billId, _billingSettings.Value.TaxRate, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var finalBill = await _billRepository.GetBillByIdAsync(billId);
            return MapToResponse(finalBill!);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static BillResponse MapToResponse(BillEntity bill)
    {
        return new BillResponse
        {
            BillId = bill.BillId,
            CreatedDate = bill.CreatedDate,
            TotalAmount = bill.TotalAmount,
            Discount = bill.Discount,
            Tax = bill.Tax,
            FinalAmount = bill.FinalAmount,
            Items = bill.BillItems.Select(bi => new BillItemResponse
            {
                BillItemId = bi.BillItemId,
                MedicineId = bi.MedicineId,
                MedicineName = bi.Medicine?.MedicineName ?? "Unknown",
                Quantity = bi.Quantity,
                UnitPrice = bi.UnitPrice,
                TotalPrice = bi.TotalPrice
            }).ToList()
        };
    }
}
