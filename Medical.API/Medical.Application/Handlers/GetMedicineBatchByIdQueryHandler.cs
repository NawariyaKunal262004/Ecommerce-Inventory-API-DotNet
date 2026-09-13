namespace Medical.Application.Handlers;
public class GetMedicineBatchByIdQueryHandler : IRequestHandler<GetMedicineBatchByIdQuery, MedicineBatchResponse>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;

    public GetMedicineBatchByIdQueryHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
    }

    public async Task<MedicineBatchResponse> Handle(GetMedicineBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.GetByIdAsync(request.BatchId);
        if (batch == null)
            return null;

        var medicine = await _medicineRepository.GetByIdAsync(batch.MedicineId);

        return new MedicineBatchResponse
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
            IsExpired = batch.ExpiryDate < DateTime.UtcNow
        };
    }
}
