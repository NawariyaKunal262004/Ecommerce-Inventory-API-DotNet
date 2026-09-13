namespace Medical.Application.Queries;
public class GetInventoryBatchesQuery : IRequest<ResponseModel>
{
    public Guid MedicineId { get; set; }
}
