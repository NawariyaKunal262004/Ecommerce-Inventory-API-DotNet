namespace Medical.Application.Handlers;
public class GetMedicineByIdQueryHandler : IRequestHandler<GetMedicineById, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public GetMedicineByIdQueryHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(GetMedicineById request, CancellationToken cancellationToken)
    {
        // Fetch the medicine by ID from the database
        var medicine = await _repository.GetByIdAsync(request.Id);
        
        // Validate that the medicine exists
        if (medicine == null)
            return ResponseModel.FailureResponse("Medicine not found");

        // Map the entity to response DTO
        var response = new MedicineResponse
        {
            MedicineId = medicine.MedicineId,
            MedicineName = medicine.MedicineName,
            MedicineCategory = medicine.MedicineCategory,
            MedicinePrice = medicine.MedicinePrice,
            Stock = medicine.Stock,
            ExpirationDate = medicine.ExpirationDate,
            SupplierID = medicine.SupplierId,
            ManufacturingDate = medicine.ManufacturingDate,
            Manufacturer = medicine.Manufacturer,
            CreatedAt = medicine.CreatedAt,
            LastUpdatedAt = medicine.LastUpdatedAt
        };

        // Return success response with the medicine data
        return ResponseModel.SuccessResponse(response, "Medicine retrieved successfully");
    }
}
