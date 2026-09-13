namespace Medical.Application.Handlers;
public class GetLowStockQueryHandler : IRequestHandler<GetLowStockQuery, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public GetLowStockQueryHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(GetLowStockQuery request, CancellationToken cancellationToken)
    {
        // Fetch all medicines with stock < 10 from repository
        // The actual filtering happens in MedicineRepository.GetLowStockAsync()
        var medicines = await _repository.GetLowStockAsync();

        // Map each entity to response DTO
        var responses = medicines.Select(m => new MedicineResponse
        {
            MedicineId = m.MedicineId,
            MedicineName = m.MedicineName,
            MedicineCategory = m.MedicineCategory,
            MedicinePrice = m.MedicinePrice,
            Stock = m.Stock,
            ExpirationDate = m.ExpirationDate,
            SupplierID = m.SupplierId,
            ManufacturingDate = m.ManufacturingDate,
            Manufacturer = m.Manufacturer,
            CreatedAt = m.CreatedAt,
            LastUpdatedAt = m.LastUpdatedAt
        }).ToList();

        // Return success response with filtered list
        return ResponseModel.SuccessResponse(responses, "Low stock medicines retrieved successfully");
    }
}
