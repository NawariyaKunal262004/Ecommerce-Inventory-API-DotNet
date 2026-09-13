namespace Medical.Infra.Repositories;
public class BillInventoryService : IBillInventoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IInventoryService _inventoryService;

    public BillInventoryService(
        ApplicationDbContext context,
        IMedicineBatchRepository batchRepository,
        IInventoryService inventoryService)
    {
        _context = context;
        _batchRepository = batchRepository;
        _inventoryService = inventoryService;
    }

    public async Task DeductStockAsync(Guid medicineId, int quantity, Guid billId, CancellationToken cancellationToken = default)
    {
        var availableBatches = await _batchRepository.GetAvailableBatchesAsync(medicineId, DateTime.UtcNow);
        if (!availableBatches.Any())
            throw new InvalidOperationException($"No available stock found for medicine {medicineId}.");

        var totalAvailable = availableBatches.Sum(b => b.QuantityAvailable);
        if (totalAvailable < quantity)
            throw new InvalidOperationException(
                $"Insufficient stock for medicine {medicineId}. Available: {totalAvailable}, Requested: {quantity}");

        var remainingToDeduct = quantity;
        foreach (var batch in availableBatches)
        {
            if (remainingToDeduct <= 0)
                break;

            var deductFromBatch = Math.Min(batch.QuantityAvailable, remainingToDeduct);
            var newQuantity = batch.QuantityAvailable - deductFromBatch;
            remainingToDeduct -= deductFromBatch;

            await SetBatchQuantityAsync(batch.Id, newQuantity, cancellationToken);
            await AddInventoryTransactionAsync(batch.Id, medicineId, -deductFromBatch, billId,
                $"Stock deducted for bill {billId}", cancellationToken);
        }

        await _inventoryService.UpdateMedicineTotalStockAsync(medicineId, saveChanges: false, cancellationToken);
    }

    public async Task RestoreStockAsync(Guid medicineId, int quantity, Guid billId, CancellationToken cancellationToken = default)
    {
        var batches = await _batchRepository.GetBatchesForStockRestoreAsync(medicineId, DateTime.UtcNow);
        if (!batches.Any())
            throw new InvalidOperationException($"No batches found for medicine {medicineId} to restore stock.");

        var remainingToRestore = quantity;
        foreach (var batch in batches)
        {
            if (remainingToRestore <= 0)
                break;

            var restoreToBatch = Math.Min(remainingToRestore, int.MaxValue - batch.QuantityAvailable);
            var newQuantity = batch.QuantityAvailable + restoreToBatch;
            remainingToRestore -= restoreToBatch;

            await SetBatchQuantityAsync(batch.Id, newQuantity, cancellationToken);
            await AddInventoryTransactionAsync(batch.Id, medicineId, restoreToBatch, billId,
                $"Stock restored for bill {billId}", cancellationToken);
        }

        if (remainingToRestore > 0)
            throw new InvalidOperationException(
                $"Could not restore all stock for medicine {medicineId}. Remaining: {remainingToRestore}.");

        await _inventoryService.UpdateMedicineTotalStockAsync(medicineId, saveChanges: false, cancellationToken);
    }

    private Task SetBatchQuantityAsync(Guid batchId, int quantity, CancellationToken cancellationToken) =>
        _context.MedicineBatches
            .Where(b => b.Id == batchId)
            .ExecuteUpdateAsync(
                s => s.SetProperty(b => b.QuantityAvailable, quantity),
                cancellationToken);

    private async Task AddInventoryTransactionAsync(
        Guid batchId,
        Guid medicineId,
        int quantity,
        Guid billId,
        string remarks,
        CancellationToken cancellationToken)
    {
        _context.InventoryTransactions.Add(new InventoryTransactionEntity
        {
            Id = Guid.NewGuid(),
            MedicineBatchId = batchId,
            MedicineId = medicineId,
            Quantity = quantity,
            TransactionDate = DateTime.UtcNow,
            ReferenceNumber = $"BILL-{billId}",
            Remarks = remarks
        });

        await Task.CompletedTask;
    }
}
