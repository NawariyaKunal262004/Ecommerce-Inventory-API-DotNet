namespace Medical.Application.Handlers;
public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierResponse>
{
    private readonly ISupplierRepository _supplierRepository;

    public CreateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        // Minimal validation
        if (string.IsNullOrWhiteSpace(request.SupplierName))
            throw new ArgumentException("Supplier name is required");
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new ArgumentException("Phone number is required");
        if (!string.IsNullOrWhiteSpace(request.Email) && !request.Email.Contains("@"))
            throw new ArgumentException("Invalid email format");

        var supplier = new SupplierEntity
        {
            SupplierName = request.SupplierName,
            ContactPerson = request.ContactPerson,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _supplierRepository.CreateSupplierAsync(supplier);

        return new SupplierResponse
        {
            SupplierId = created.SupplierId,
            SupplierName = created.SupplierName,
            ContactPerson = created.ContactPerson,
            PhoneNumber = created.PhoneNumber,
            Email = created.Email,
            Address = created.Address,
            City = created.City,
            State = created.State,
            PostalCode = created.PostalCode,
            Country = created.Country,
            CreatedAt = created.CreatedAt,
            LastUpdatedAt = created.LastUpdatedAt,
            IsActive = created.IsActive
        };
    }
}
