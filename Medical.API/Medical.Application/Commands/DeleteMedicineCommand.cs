namespace Medical.Application.Commands;
public class DeleteMedicineCommand : IRequest<ResponseModel>
{
    public Guid MedicineId { get; set; }
}
