namespace Medical.Application.Handlers;
public class GetSupplierQueryHandler : IRequestHandler<GetSupplierQuery, SupplierResponse>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierQueryHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierResponse> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetSupplierByIdAsync(request.SupplierId);
        if (supplier == null)
            throw new KeyNotFoundException("Supplier not found");
        return new SupplierResponse
        {
            SupplierId = supplier.SupplierId,
            SupplierName = supplier.SupplierName,
            ContactPerson = supplier.ContactPerson,
            PhoneNumber = supplier.PhoneNumber,
            Email = supplier.Email,
            Address = supplier.Address,
            City = supplier.City,
            State = supplier.State,
            PostalCode = supplier.PostalCode,
            Country = supplier.Country,
            CreatedAt = supplier.CreatedAt,
            LastUpdatedAt = supplier.LastUpdatedAt,
            IsActive = supplier.IsActive
        };
    }
}
