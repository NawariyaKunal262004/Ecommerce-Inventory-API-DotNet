namespace Medical.Application.Handlers;
public class AddBillCommandHandler : IRequestHandler<AddBillCommand, BillResponse>
{
    private readonly IBillRepository _billRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IBillInventoryService _billInventoryService;
    private readonly IBillService _billService;
    private readonly IOptions<BillingSettings> _billingSettings;
    private readonly ApplicationDbContext _context;

    public AddBillCommandHandler(
        IBillRepository billRepository,
        IMedicineRepository medicineRepository,
        IBillInventoryService billInventoryService,
        IBillService billService,
        IOptions<BillingSettings> billingSettings,
        ApplicationDbContext context)
    {
        _billRepository = billRepository;
        _medicineRepository = medicineRepository;
        _billInventoryService = billInventoryService;
        _billService = billService;
        _billingSettings = billingSettings;
        _context = context;
    }

    public async Task<BillResponse> Handle(AddBillCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var medicine = await _medicineRepository.GetByIdAsync(request.MedicineId);
            if (medicine == null)
                throw new InvalidOperationException($"Medicine with ID {request.MedicineId} not found.");

            if (medicine.ExpirationDate < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new InvalidOperationException($"Medicine {medicine.MedicineName} has expired.");

            await _billInventoryService.DeductStockAsync(
                request.MedicineId, request.Quantity, request.BillId, cancellationToken);

            var billItem = new BillItemEntity
            {
                BillItemId = Guid.NewGuid(),
                BillId = request.BillId,
                MedicineId = request.MedicineId,
                Quantity = request.Quantity,
                UnitPrice = medicine.MedicinePrice,
                TotalPrice = request.Quantity * medicine.MedicinePrice
            };

            await _billService.AddBillItemAsync(billItem, cancellationToken);
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
