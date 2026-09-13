namespace Medical.Application.Handlers;
public class RemoveExpiredStockCommandHandler : IRequestHandler<RemoveExpiredStockCommand, ResponseModel>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IInventoryService _inventoryService;

    public RemoveExpiredStockCommandHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository,
        IInventoryService inventoryService)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
        _inventoryService = inventoryService;
    }

    public async Task<ResponseModel> Handle(RemoveExpiredStockCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.MedicineBatchId);
        if (batch == null)
            throw new MedicineBatchNotFoundException();

        var medicine = await _medicineRepository.GetByIdAsync(batch.MedicineId);

        batch.QuantityAvailable = 0;
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
            SupplierId = batch.SupplierId,
            IsExpired = true
        };

        return ResponseModel.SuccessResponse(response, 
            $"Successfully removed expired stock from batch {batch.BatchNumber} of medicine {medicine?.MedicineName ?? "Unknown"}");
    }
}
