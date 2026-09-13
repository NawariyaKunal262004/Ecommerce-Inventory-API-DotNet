namespace Medical.Application.Handlers;
public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, bool>
{
    private readonly ISupplierRepository _supplierRepository;

    public DeleteSupplierCommandHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetSupplierByIdAsync(request.SupplierId);
        if (supplier == null)
            throw new InvalidOperationException($"Supplier with ID {request.SupplierId} not found.");

        if (supplier.Medicines.Any())
            throw new InvalidOperationException($"Cannot delete supplier with ID {request.SupplierId} because they have associated medicines.");

        var result = await _supplierRepository.DeleteSupplierAsync(request.SupplierId);
        return result;
    }
}
