namespace Medical.Application.Commands;
public class AddBillCommand : IRequest<BillResponse>
{
    public Guid BillId { get; set; }

    public Guid MedicineId { get; set; }

    public int Quantity { get; set; }
}
