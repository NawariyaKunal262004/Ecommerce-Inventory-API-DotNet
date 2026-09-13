namespace Medical.Application.Handlers;
public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, ResponseModel>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IInventoryService _inventoryService;

    public AdjustStockCommandHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository,
        IInventoryService inventoryService)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
        _inventoryService = inventoryService;
    }

    public async Task<ResponseModel> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.MedicineBatchId);
        if (batch == null)
            throw new MedicineBatchNotFoundException();

        if (request.AdjustedQuantity < 0)
            throw new InvalidStockAdjustmentException();

        var medicine = await _medicineRepository.GetByIdAsync(batch.MedicineId);

        batch.QuantityAvailable = request.AdjustedQuantity;
        await _batchRepository.UpdateAsync(batch);
        await _inventoryService.UpdateMedicineTotalStockAsync(batch.MedicineId);

        var response = new MedicineBatchResponse
        {
            Id = batch.Id,
            MedicineId = batch.MedicineId,
            MedicineName = medicine?.MedicineName,
            BatchNumber = batch.BatchNumber,
            QuantityAvailable = batch.QuantityAvailable,
            PurchasePrice = batch.PurchasePrice,
            SellingPrice = batch.SellingPrice,
            ManufacturingDate = batch.ManufacturingDate,
            ExpiryDate = batch.ExpiryDate,
            SupplierId = batch.SupplierId
        };

        return ResponseModel.SuccessResponse(response, 
            $"Stock adjusted successfully for batch {batch.BatchNumber} of medicine {medicine?.MedicineName ?? "Unknown"}");
    }
}
