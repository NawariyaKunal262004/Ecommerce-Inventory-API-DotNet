namespace Medical.Core.IRepositories;
public interface IInventoryService
{
    Task UpdateMedicineTotalStockAsync(Guid medicineId, bool saveChanges = true, CancellationToken cancellationToken = default);
    Task<bool> IsStockAvailableAsync(Guid medicineId, int quantity);
}
