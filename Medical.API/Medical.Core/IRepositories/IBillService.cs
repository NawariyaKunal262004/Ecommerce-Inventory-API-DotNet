namespace Medical.Core.IRepositories;
public interface IBillService
{
    Task RecalculateAndPersistTotalsAsync(Guid billId, decimal taxRate, CancellationToken cancellationToken = default);

    Task UpdateDiscountAsync(Guid billId, decimal discount, CancellationToken cancellationToken = default);

    Task AddBillItemAsync(BillItemEntity billItem, CancellationToken cancellationToken = default);

    Task UpdateBillItemAsync(Guid billItemId, int quantity, decimal totalPrice, CancellationToken cancellationToken = default);

    Task DeleteBillItemAsync(Guid billItemId, CancellationToken cancellationToken = default);

    Task DeleteBillAsync(Guid billId, CancellationToken cancellationToken = default);
}
