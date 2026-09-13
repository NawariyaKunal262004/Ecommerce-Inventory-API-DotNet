namespace Medical.Application.Handlers;
public class RestoreStockCommandHandler : IRequestHandler<RestoreStockCommand, ResponseModel>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IInventoryService _inventoryService;

    public RestoreStockCommandHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository,
        IInventoryService inventoryService)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
        _inventoryService = inventoryService;
    }

    public async Task<ResponseModel> Handle(RestoreStockCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.MedicineBatchId);
        if (batch == null)
            throw new MedicineBatchNotFoundException();

        var medicine = await _medicineRepository.GetByIdAsync(batch.MedicineId);

        batch.QuantityAvailable += request.Quantity;
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
            $"Successfully restored {request.Quantity} units to batch {batch.BatchNumber} of medicine {medicine?.MedicineName ?? "Unknown"}");
    }
}
