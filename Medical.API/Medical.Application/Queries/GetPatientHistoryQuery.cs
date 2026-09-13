namespace Medical.Application.Queries;
public class GetPatientHistoryQuery : IRequest<PatientHistoryResponse>
{
    public GetPatientHistoryQuery(Guid patientId)
    {
        PatientId = patientId;
    }

    public Guid PatientId { get; set; }
}
