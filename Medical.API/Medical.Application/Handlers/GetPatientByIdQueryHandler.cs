namespace Medical.Application.Handlers;
public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientResponse>
{
    private readonly IPatientRepository _repository;

    public GetPatientByIdQueryHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<PatientResponse> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        // Fetch the patient by ID from the database
        var patient = await _repository.GetByIdAsync(request.PatientId);

        // Validate that the patient exists
        if (patient == null)
            throw new InvalidOperationException($"Patient with ID {request.PatientId} not found.");

        // Calculate age from date of birth
        var age = CalculateAge(patient.DateOfBirth);

        // Map the entity to response DTO
        var response = new PatientResponse
        {
            PatientId = patient.PatientId,
            PatientName = patient.PatientName,
            DateOfBirth = patient.DateOfBirth,
            Age = age,
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
