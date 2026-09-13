namespace Medical.Application.Commands;
public class DeleteSupplierCommand : IRequest<bool>
{
    public Guid SupplierId { get; set; }
}
