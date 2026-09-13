namespace Medical.Application.Responses;
public class LowStockMedicineResponse
{
    public Guid MedicineId { get; set; }

    public string? MedicineName { get; set; }

    public int AvailableQuantity { get; set; }

    public int Threshold { get; set; }
}
