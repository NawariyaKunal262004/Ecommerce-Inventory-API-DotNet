namespace Medical.Application.Queries;
public class GetStockAuditQuery : IRequest<ResponseModel>
{
    public Guid? MedicineId { get; set; }
}
