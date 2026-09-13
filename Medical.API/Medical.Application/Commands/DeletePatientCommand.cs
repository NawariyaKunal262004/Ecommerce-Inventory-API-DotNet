namespace Medical.Application.Commands;
public class DeletePatientCommand : IRequest<bool>
{
    public Guid PatientId { get; set; }
}
