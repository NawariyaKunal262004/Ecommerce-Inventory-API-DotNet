namespace Medical.Core.IRepositories;
public interface IAsyncRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task<bool> DeleteAsync(Guid id);
}

internal interface IAsyncRepository : IAsyncRepository<MedicineEntity>
{
}
