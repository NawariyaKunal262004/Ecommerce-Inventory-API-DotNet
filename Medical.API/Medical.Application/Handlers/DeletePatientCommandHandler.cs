namespace Medical.Application.Handlers;
public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
{
    private readonly IPatientRepository _patientRepository;

    public DeletePatientCommandHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetPatientWithBillsAsync(request.PatientId);
        if (patient == null)
            throw new InvalidOperationException($"Patient with ID {request.PatientId} not found.");

        if (patient.Bills.Any())
            throw new InvalidOperationException($"Cannot delete patient with ID {request.PatientId} because they have associated bills.");

        await _patientRepository.DeleteAsync(request.PatientId);
        return true;
    }
}
