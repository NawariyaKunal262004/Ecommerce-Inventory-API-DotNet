namespace Medical.Application.Handlers;
public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand, BillResponse>
{
    private readonly IBillRepository _billRepository;
    private readonly IBillService _billService;
    private readonly IOptions<BillingSettings> _billingSettings;
    private readonly ApplicationDbContext _context;

    public UpdateBillCommandHandler(
        IBillRepository billRepository,
        IBillService billService,
        IOptions<BillingSettings> billingSettings,
        ApplicationDbContext context)
    {
        _billRepository = billRepository;
        _billService = billService;
        _billingSettings = billingSettings;
        _context = context;
    }

    public async Task<BillResponse> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.Discount.HasValue && request.Discount.Value < 0)
                throw new InvalidBillOperationException("Discount cannot be negative.");

            var bill = await _billRepository.GetBillByIdAsync(request.BillId);
            if (bill == null)
                throw new BillNotFoundException($"Bill with ID {request.BillId} not found.");

            if (request.Discount.HasValue)
                await _billService.UpdateDiscountAsync(request.BillId, request.Discount.Value, cancellationToken);

            await _billService.RecalculateAndPersistTotalsAsync(
                request.BillId, _billingSettings.Value.TaxRate, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var finalBill = await _billRepository.GetBillByIdAsync(request.BillId);
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
