namespace Medical.Application.Handlers;
public class DeleteMedicineCommandHandler : IRequestHandler<DeleteMedicineCommand, ResponseModel>
{
    private readonly IMedicineRepository _repository;

    public DeleteMedicineCommandHandler(IMedicineRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
    {
        // Call repository to delete the medicine by ID
        // Repository returns true if deletion was successful, false if medicine not found
        var deleted = await _repository.DeleteAsync(request.MedicineId);
        
        // Check if deletion was successful
        if (!deleted)
            return ResponseModel.FailureResponse("Medicine not found");

        // Return success response (no data for delete operations)
        return ResponseModel.SuccessResponse(null, "Medicine deleted successfully");
    }
}
