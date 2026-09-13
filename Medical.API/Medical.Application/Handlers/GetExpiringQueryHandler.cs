namespace Medical.Application.Handlers;
public class GetExpiringQueryHandler : IRequestHandler<GetExpiringQuery, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public GetExpiringQueryHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(GetExpiringQuery request, CancellationToken cancellationToken)
    {
        // Fetch all medicines expiring within 30 days from repository
        // The actual filtering happens in MedicineRepository.GetExpiringAsync()
        var medicines = await _repository.GetExpiringAsync();

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
        return ResponseModel.SuccessResponse(responses, "Expiring medicines retrieved successfully");
    }
}
