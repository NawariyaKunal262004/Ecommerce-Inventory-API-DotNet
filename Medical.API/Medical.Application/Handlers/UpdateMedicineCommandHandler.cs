namespace Medical.Application.Handlers;
public class UpdateMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public UpdateMedicineCommandHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
    {
        // Fetch the existing medicine record from database
        var existingMedicine = await _repository.GetByIdAsync(request.MedicineId);
        
        // Validate that the medicine exists
        if (existingMedicine == null)
            return ResponseModel.FailureResponse("Medicine not found");

        // Update each property with new values from the command
        existingMedicine.MedicineName = request.MedicineName;
        existingMedicine.MedicineCategory = request.MedicineCategory;
        existingMedicine.MedicinePrice = request.MedicinePrice;
        existingMedicine.ExpirationDate = request.ExpirationDate;
        existingMedicine.SupplierId = request.SupplierID;
        existingMedicine.ManufacturingDate = request.ManufacturingDate;
        existingMedicine.Manufacturer = request.Manufacturer;
        // Update the LastUpdatedAt timestamp (audit trail)
        existingMedicine.LastUpdatedAt = DateTime.UtcNow;

        // Persist the updated medicine to the database
        var updatedMedicine = await _repository.UpdateAsync(existingMedicine);

        // Map the updated entity to response DTO
        var response = new MedicineResponse
        {
            MedicineId = updatedMedicine.MedicineId,
            MedicineName = updatedMedicine.MedicineName,
            MedicineCategory = updatedMedicine.MedicineCategory,
            MedicinePrice = updatedMedicine.MedicinePrice,
            Stock = updatedMedicine.Stock,
            ExpirationDate = updatedMedicine.ExpirationDate,
            SupplierID = updatedMedicine.SupplierId,
            ManufacturingDate = updatedMedicine.ManufacturingDate,
            Manufacturer = updatedMedicine.Manufacturer,
            CreatedAt = updatedMedicine.CreatedAt,
            LastUpdatedAt = updatedMedicine.LastUpdatedAt
        };

        return ResponseModel.SuccessResponse(response, "Medicine updated successfully");
    }
}
