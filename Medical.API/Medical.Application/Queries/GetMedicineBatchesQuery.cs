namespace Medical.Application.Queries;
public class GetMedicineBatchesQuery : IRequest<ResponseModel>
{
    public Guid MedicineId { get; set; }
}
