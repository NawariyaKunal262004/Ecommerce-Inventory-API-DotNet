namespace Medical.Application.Responses;
public class ExpiringMedicineResponse
{
    public Guid MedicineId { get; set; }
    public string? MedicineName { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int Quantity { get; set; }
}
