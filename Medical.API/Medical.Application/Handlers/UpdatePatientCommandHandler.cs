namespace Medical.Application.Handlers;
public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, PatientResponse>
{
    private readonly IPatientRepository _patientRepository;

    public UpdatePatientCommandHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientResponse> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.PatientId);
        if (patient == null)
            throw new InvalidOperationException($"Patient with ID {request.PatientId} not found.");

        patient.PatientName = request.PatientName ?? patient.PatientName;
        patient.DateOfBirth = request.DateOfBirth;
        patient.Gender = request.Gender;
        patient.PhoneNumber = request.PhoneNumber ?? patient.PhoneNumber;
        patient.Email = request.Email ?? patient.Email;
        patient.Address = request.Address ?? patient.Address;
        patient.City = request.City ?? patient.City;
        patient.State = request.State ?? patient.State;
        patient.PostalCode = request.PostalCode ?? patient.PostalCode;
        patient.Country = request.Country ?? patient.Country;
        patient.EmergencyContactName = request.EmergencyContactName ?? patient.EmergencyContactName;
        patient.EmergencyContactPhone = request.EmergencyContactPhone ?? patient.EmergencyContactPhone;
        patient.LastUpdatedAt = DateTime.UtcNow;

        await _patientRepository.UpdateAsync(patient);

        return MapToResponse(patient);
    }

    private PatientResponse MapToResponse(PatientEntity patient)
    {
        return new PatientResponse
        {
            PatientId = patient.PatientId,
            PatientName = patient.PatientName,
            DateOfBirth = patient.DateOfBirth,
            Age = CalculateAge(patient.DateOfBirth),
            Gender = patient.Gender,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            Address = patient.Address,
            City = patient.City,
            State = patient.State,
            PostalCode = patient.PostalCode,
            Country = patient.Country,
            EmergencyContactName = patient.EmergencyContactName,
            EmergencyContactPhone = patient.EmergencyContactPhone,
            CreatedAt = patient.CreatedAt,
            LastUpdatedAt = patient.LastUpdatedAt
        };
    }

    private int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (today.DayOfYear < dateOfBirth.DayOfYear)
            age--;
        return age;
    }
}
