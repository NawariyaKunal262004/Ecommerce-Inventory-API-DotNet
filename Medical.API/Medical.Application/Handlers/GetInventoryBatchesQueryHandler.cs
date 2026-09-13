namespace Medical.Application.Handlers;
public class GetInventoryBatchesQueryHandler : IRequestHandler<GetInventoryBatchesQuery, ResponseModel>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;

    public GetInventoryBatchesQueryHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
    }

    public async Task<ResponseModel> Handle(GetInventoryBatchesQuery request, CancellationToken cancellationToken)
    {
        var batches = await _batchRepository.GetBatchesByMedicineIdAsync(request.MedicineId);
        var medicine = await _medicineRepository.GetByIdAsync(request.MedicineId);

        var batchResponses = batches.Select(b => new MedicineBatchResponse
        {
            Id = b.Id,
            MedicineId = b.MedicineId,
            MedicineName = medicine?.MedicineName,
            BatchNumber = b.BatchNumber,
            QuantityAvailable = b.QuantityAvailable,
            PurchasePrice = b.PurchasePrice,
            SellingPrice = b.SellingPrice,
            ManufacturingDate = b.ManufacturingDate,
            ExpiryDate = b.ExpiryDate,
            SupplierId = b.SupplierId,
            IsExpired = b.ExpiryDate < DateTime.UtcNow
        }).ToList();

        return ResponseModel.SuccessResponse(batchResponses, 
            $"Retrieved {batchResponses.Count} batches for medicine {medicine?.MedicineName ?? request.MedicineId.ToString()}");
    }
}
