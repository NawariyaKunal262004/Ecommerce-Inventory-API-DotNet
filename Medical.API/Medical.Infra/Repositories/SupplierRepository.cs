namespace Medical.Infra.Repositories;
public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _context;

    public SupplierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SupplierEntity> CreateSupplierAsync(SupplierEntity supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<SupplierEntity?> GetSupplierByIdAsync(Guid supplierId)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierId);
    }

    public async Task<List<SupplierEntity>> GetAllSuppliersAsync()
    {
        return await _context.Suppliers.ToListAsync();
    }

    public async Task<SupplierEntity?> UpdateSupplierAsync(SupplierEntity supplier)
    {
        var existing = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplier.SupplierId);
        if (existing == null) return null;
        existing.SupplierName = supplier.SupplierName;
        existing.ContactPerson = supplier.ContactPerson;
        existing.PhoneNumber = supplier.PhoneNumber;
        existing.Email = supplier.Email;
        existing.Address = supplier.Address;
        existing.City = supplier.City;
        existing.State = supplier.State;
        existing.PostalCode = supplier.PostalCode;
        existing.Country = supplier.Country;
        existing.LastUpdatedAt = DateTime.UtcNow;
        existing.IsActive = supplier.IsActive;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteSupplierAsync(Guid supplierId)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplierId);
        if (supplier == null) return false;
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return true;
    }
}
