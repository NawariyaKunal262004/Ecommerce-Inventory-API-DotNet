namespace Medical.Core.IRepositories;
public interface ISupplierRepository
{
    Task<SupplierEntity> CreateSupplierAsync(SupplierEntity supplier);
    Task<SupplierEntity?> GetSupplierByIdAsync(Guid supplierId);
    Task<List<SupplierEntity>> GetAllSuppliersAsync();
    Task<SupplierEntity?> UpdateSupplierAsync(SupplierEntity supplier);
    Task<bool> DeleteSupplierAsync(Guid supplierId);
}
