namespace Medical.Application.Handlers;
public class GetLowStockMedicinesQueryHandler : IRequestHandler<GetLowStockMedicinesQuery, ResponseModel>
{
    private readonly IMedicineRepository _medicineRepository;

    public GetLowStockMedicinesQueryHandler(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository;
    }

    public async Task<ResponseModel> Handle(GetLowStockMedicinesQuery request, CancellationToken cancellationToken)
    {
        var allMedicines = await _medicineRepository.GetAllAsync();
        var lowStockMedicines = allMedicines
            .Where(m => m.Stock < request.Threshold)
            .Select(m => new LowStockMedicineResponse
            {
                MedicineId = m.MedicineId,
                MedicineName = m.MedicineName,
                AvailableQuantity = m.Stock,
                Threshold = request.Threshold
            })
            .ToList();

        return ResponseModel.SuccessResponse(lowStockMedicines, $"Found {lowStockMedicines.Count} medicines below threshold of {request.Threshold}");
    }
}
