namespace Medical.Application.Handlers;
public class DeductStockCommandHandler : IRequestHandler<DeductStockCommand, ResponseModel>
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly IBillInventoryService _billInventoryService;
    private readonly ApplicationDbContext _context;

    public DeductStockCommandHandler(
        IMedicineRepository medicineRepository,
        IBillInventoryService billInventoryService,
        ApplicationDbContext context)
    {
        _medicineRepository = medicineRepository;
        _billInventoryService = billInventoryService;
        _context = context;
    }

    public async Task<ResponseModel> Handle(DeductStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _billInventoryService.DeductStockAsync(
                request.MedicineId, request.Quantity, request.BillId, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Insufficient stock") || ex.Message.Contains("No available stock"))
        {
            throw new InsufficientStockException(ex.Message);
        }

        var medicine = await _medicineRepository.GetByIdAsync(request.MedicineId);
        if (medicine != null && medicine.Stock < 10)
        {
            _ = new LowStockDetectedEvent
            {
                MedicineId = request.MedicineId,
                AvailableQuantity = medicine.Stock,
                Threshold = 10,
                DetectedAt = DateTime.UtcNow
            };
        }

        var response = new
        {
            MedicineId = request.MedicineId,
            MedicineName = medicine?.MedicineName,
            QuantityDeducted = request.Quantity,
            RemainingStock = medicine?.Stock
        };

        return ResponseModel.SuccessResponse(response,
            $"Successfully deducted {request.Quantity} units of {medicine?.MedicineName ?? "medicine"} using FIFO logic");
    }
}
