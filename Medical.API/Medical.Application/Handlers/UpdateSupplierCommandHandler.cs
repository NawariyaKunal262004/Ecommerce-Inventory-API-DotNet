namespace Medical.Application.Handlers;
public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, SupplierResponse>
{
    private readonly ISupplierRepository _supplierRepository;

    public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetSupplierByIdAsync(request.SupplierId);
        if (supplier == null)
            throw new InvalidOperationException($"Supplier with ID {request.SupplierId} not found.");

        supplier.SupplierName = request.SupplierName;
        supplier.ContactPerson = request.ContactPerson ?? supplier.ContactPerson;
        supplier.PhoneNumber = request.PhoneNumber ?? supplier.PhoneNumber;
        supplier.Email = request.Email ?? supplier.Email;
        supplier.Address = request.Address ?? supplier.Address;
        supplier.City = request.City ?? supplier.City;
        supplier.State = request.State ?? supplier.State;
        supplier.PostalCode = request.PostalCode ?? supplier.PostalCode;
        supplier.Country = request.Country ?? supplier.Country;
        supplier.IsActive = request.IsActive;
        supplier.LastUpdatedAt = DateTime.UtcNow;

        var updatedSupplier = await _supplierRepository.UpdateSupplierAsync(supplier);
        return MapToResponse(updatedSupplier);
    }

    private SupplierResponse MapToResponse(SupplierEntity supplier)
    {
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
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt,
            LastUpdatedAt = supplier.LastUpdatedAt
        };
    }
}
