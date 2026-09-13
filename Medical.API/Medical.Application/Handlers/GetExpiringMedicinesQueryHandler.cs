namespace Medical.Application.Handlers;
public class GetExpiringMedicinesQueryHandler : IRequestHandler<GetExpiringMedicinesQuery, ResponseModel>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;

    public GetExpiringMedicinesQueryHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
    }

    public async Task<ResponseModel> Handle(GetExpiringMedicinesQuery request, CancellationToken cancellationToken)
    {
        var expiryThreshold = DateTime.UtcNow.AddDays(30);
        var expiringBatches = await _batchRepository.GetExpiringBatchesAsync(expiryThreshold);

        var medicines = await _medicineRepository.GetAllAsync();
        var medicineDict = medicines.ToDictionary(m => m.MedicineId, m => m.MedicineName);

        var expiringMedicines = expiringBatches
            .Select(b => new ExpiringMedicineResponse
            {
                MedicineId = b.MedicineId,
                MedicineName = medicineDict.ContainsKey(b.MedicineId) 
                    ? medicineDict[b.MedicineId] 
                    : "Unknown",
                ExpiryDate = b.ExpiryDate,
                Quantity = b.QuantityAvailable
            })
            .ToList();

        return ResponseModel.SuccessResponse(expiringMedicines, 
            $"Found {expiringMedicines.Count} batches expiring within 30 days");
    }
}
