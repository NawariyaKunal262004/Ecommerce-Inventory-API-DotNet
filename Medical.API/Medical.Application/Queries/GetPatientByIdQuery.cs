namespace Medical.Application.Queries;
public class GetPatientByIdQuery : IRequest<PatientResponse>
{
    public GetPatientByIdQuery(Guid patientId)
    {
        PatientId = patientId;
    }

    public Guid PatientId { get; set; }
}
