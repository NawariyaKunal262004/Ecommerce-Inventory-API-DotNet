namespace Medical.Application.Handlers;
public class CreateMedicineCommandHandler : IRequestHandler<CreateMedicineCommand, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public CreateMedicineCommandHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(CreateMedicineCommand request, CancellationToken cancellationToken)
    {
        // Map command properties to a new MedicineEntity
        // Note: Stock is initialized to 0 (can be updated separately)
        // CreatedAt and LastUpdatedAt are set to current UTC time
        var medicine = new MedicineEntity
        {
            MedicineId = Guid.NewGuid(),
            MedicineName = request.MedicineName,
            MedicineCategory = request.MedicineCategory,
            MedicinePrice = request.MedicinePrice,
            Stock = 0,  // New medicines start with 0 stock
            ExpirationDate = request.ExpirationDate,
            SupplierId = request.SupplierId,
            ManufacturingDate = request.ManufacturingDate,
            Manufacturer = request.Manufacturer,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        // Persist the new medicine to the database via repository
        var createdMedicine = await _repository.AddAsync(medicine);

        // Map the created entity to response DTO for API response
        var response = new MedicineResponse
        {
            MedicineId = createdMedicine.MedicineId,
            MedicineName = createdMedicine.MedicineName,
            MedicineCategory = createdMedicine.MedicineCategory,
            MedicinePrice = createdMedicine.MedicinePrice,
            Stock = createdMedicine.Stock,
            ExpirationDate = createdMedicine.ExpirationDate,
            SupplierID = createdMedicine.SupplierId,
            ManufacturingDate = createdMedicine.ManufacturingDate,
            Manufacturer = createdMedicine.Manufacturer,
            CreatedAt = createdMedicine.CreatedAt,
            LastUpdatedAt = createdMedicine.LastUpdatedAt
        };

        // Return successful response with the created medicine data
        return ResponseModel.SuccessResponse(response, "Medicine created successfully");
    }
}
