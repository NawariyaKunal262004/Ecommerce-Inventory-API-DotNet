namespace Medical.Application.Queries;
public class GetSuppliersQuery : IRequest<List<SupplierResponse>>
{
}

public class GetSupplierQuery : IRequest<SupplierResponse>
{
    public Guid SupplierId { get; set; }
    public GetSupplierQuery(Guid supplierId)
    {
        SupplierId = supplierId;
    }
}
