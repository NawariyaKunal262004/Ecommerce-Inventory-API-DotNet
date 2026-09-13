namespace Medical.Core.IRepositories;
public interface IBillRepository
{
    Task<BillEntity> CreateBillAsync(BillEntity bill);

    Task<BillEntity> AddBillItemAsync(Guid billId, BillItemEntity billItem);

    Task<BillEntity?> GetBillByIdAsync(Guid billId);

    Task<BillEntity> UpdateAsync(BillEntity bill);

    Task<decimal> GetDailySalesAsync(DateTime date);

    Task<IReadOnlyList<BillEntity>> GetAllBillsWithItemsAsync(CancellationToken cancellationToken = default);

    Task<BillItemEntity?> GetBillItemByIdAsync(Guid billItemId);

    Task DeleteBillAsync(Guid billId);

    Task DeleteBillItemAsync(Guid billItemId);
}
