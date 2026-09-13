namespace Medical.Core.IRepositories;
public interface IInventoryTransactionRepository
{
    Task AddAsync(InventoryTransactionEntity transaction);
    Task<IReadOnlyList<InventoryTransactionEntity>> GetByMedicineIdAsync(Guid medicineId);
    Task<IReadOnlyList<InventoryTransactionEntity>> GetAllAsync();
}
