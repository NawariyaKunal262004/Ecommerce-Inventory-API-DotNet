namespace Medical.Application.Commands;
public class DeductStockCommand : IRequest<ResponseModel>
{
    public Guid MedicineId { get; set; }

    public int Quantity { get; set; }

    public Guid BillId { get; set; }
}
