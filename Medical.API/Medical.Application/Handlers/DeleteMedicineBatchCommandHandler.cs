namespace Medical.Application.Handlers;
public class DeleteMedicineBatchCommandHandler : IRequestHandler<DeleteMedicineBatchCommand, ResponseModel>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IInventoryService _inventoryService;

    public DeleteMedicineBatchCommandHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository,
        IInventoryService inventoryService)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
        _inventoryService = inventoryService;
    }

    public async Task<ResponseModel> Handle(DeleteMedicineBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.Id);
        if (batch == null)
            throw new MedicineBatchNotFoundException();

        var medicine = await _medicineRepository.GetByIdAsync(batch.MedicineId);

        await _batchRepository.DeleteAsync(request.Id);
        await _inventoryService.UpdateMedicineTotalStockAsync(batch.MedicineId);

        var response = new
        {
            DeletedBatchId = request.Id,
            BatchNumber = batch.BatchNumber,
            MedicineId = batch.MedicineId,
            MedicineName = medicine?.MedicineName
        };

        return ResponseModel.SuccessResponse(response, 
            $"Successfully deleted batch {batch.BatchNumber} of medicine {medicine?.MedicineName ?? "Unknown"}");
    }
}
