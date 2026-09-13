namespace Medical.Core.IRepositories;
public interface IMedicineRepository : IAsyncRepository<MedicineEntity>
{
    Task<IReadOnlyList<MedicineEntity>> GetLowStockAsync();

    Task<IReadOnlyList<MedicineEntity>> GetExpiringAsync();
}
