namespace Medical.Application.DTOs;

public class AddBillItemRequest
{
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
}
