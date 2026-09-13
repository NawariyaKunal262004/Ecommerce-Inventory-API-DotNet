namespace Medical.Core.IRepositories;
public interface IBillInventoryService
{
    Task DeductStockAsync(Guid medicineId, int quantity, Guid billId, CancellationToken cancellationToken = default);

    Task RestoreStockAsync(Guid medicineId, int quantity, Guid billId, CancellationToken cancellationToken = default);
}
