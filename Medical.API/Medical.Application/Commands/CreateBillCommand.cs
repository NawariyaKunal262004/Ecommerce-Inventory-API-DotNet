namespace Medical.Application.Commands;
public class CreateBillCommand : IRequest<BillResponse>
{
    public Guid PatientId { get; set; }
}
