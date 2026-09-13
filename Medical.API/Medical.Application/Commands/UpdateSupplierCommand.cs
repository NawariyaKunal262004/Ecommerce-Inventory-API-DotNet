namespace Medical.Application.Commands;
public class UpdateSupplierCommand : IRequest<SupplierResponse>
{
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = null!;
    public string? ContactPerson { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public bool IsActive { get; set; }
}
