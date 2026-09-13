namespace Medical.Application.Handlers;
public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientResponse>
{
    private readonly IPatientRepository _repository;

    public CreatePatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<PatientResponse> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        // Map command properties to a new PatientEntity
        var patient = new PatientEntity
        {
            PatientId = Guid.NewGuid(),
            PatientName = request.PatientName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        // Persist the new patient to the database via repository
        var createdPatient = await _repository.AddAsync(patient);

        // Calculate age from date of birth
        var age = CalculateAge(createdPatient.DateOfBirth);

        // Map the created entity to response DTO for API response
        var response = new PatientResponse
        {
            PatientId = createdPatient.PatientId,
            PatientName = createdPatient.PatientName,
            DateOfBirth = createdPatient.DateOfBirth,
            Age = age,
            Gender = createdPatient.Gender,
            PhoneNumber = createdPatient.PhoneNumber,
            Email = createdPatient.Email,
            Address = createdPatient.Address,
            City = createdPatient.City,
            State = createdPatient.State,
            PostalCode = createdPatient.PostalCode,
            Country = createdPatient.Country,
            EmergencyContactName = createdPatient.EmergencyContactName,
            EmergencyContactPhone = createdPatient.EmergencyContactPhone,
            CreatedAt = createdPatient.CreatedAt,
            LastUpdatedAt = createdPatient.LastUpdatedAt
        };

        return response;
    }

    private int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}
