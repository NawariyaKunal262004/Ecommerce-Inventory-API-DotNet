namespace Medical.Application.Handlers;
public class GetAllMedicineListQueryHandler : IRequestHandler<GetAllMedicineListQuery, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public GetAllMedicineListQueryHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(GetAllMedicineListQuery request, CancellationToken cancellationToken)
    {
        // Fetch all medicines from the database
        var medicines = await _repository.GetAllAsync();

        // Map each entity to response DTO
        // We use manual mapping (not AutoMapper) to keep it explicit and simple
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

        // Return success response with the list of medicines
        return ResponseModel.SuccessResponse(responses, "Medicines retrieved successfully");
    }
}
