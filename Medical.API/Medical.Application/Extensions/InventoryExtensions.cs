namespace Medical.Application.Extensions;
public static class InventoryExtensions
{
    public static bool IsExpired(this MedicineBatchEntity batch)
    {
        return batch.ExpiryDate <= DateTime.UtcNow;
    }
}
