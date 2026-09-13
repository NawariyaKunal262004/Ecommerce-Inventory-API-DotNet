namespace Medical.Application.Handlers;
public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientResponse>>
{
    private readonly IPatientRepository _patientRepository;

    public GetAllPatientsQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<List<PatientResponse>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllPatientsAsync();
        return patients.Select(MapToResponse).ToList();
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
