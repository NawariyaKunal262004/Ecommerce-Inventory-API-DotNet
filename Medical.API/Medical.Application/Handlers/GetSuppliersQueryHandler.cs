namespace Medical.Application.Handlers;
public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<SupplierResponse>>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSuppliersQueryHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<List<SupplierResponse>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _supplierRepository.GetAllSuppliersAsync();
        return suppliers.Select(s => new SupplierResponse
        {
            SupplierId = s.SupplierId,
            SupplierName = s.SupplierName,
            ContactPerson = s.ContactPerson,
            PhoneNumber = s.PhoneNumber,
            Email = s.Email,
            Address = s.Address,
            City = s.City,
            State = s.State,
            PostalCode = s.PostalCode,
            Country = s.Country,
            CreatedAt = s.CreatedAt,
            LastUpdatedAt = s.LastUpdatedAt,
            IsActive = s.IsActive
        }).ToList();
    }
}
