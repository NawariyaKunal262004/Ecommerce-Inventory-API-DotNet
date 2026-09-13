namespace Medical.Application.Handlers;
public class AddMedicineBatchCommandHandler : IRequestHandler<AddMedicineBatchCommand, Guid>
{
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IInventoryTransactionRepository _transactionRepository;
    private readonly IInventoryService _inventoryService;

    public AddMedicineBatchCommandHandler(
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository,
        IInventoryTransactionRepository transactionRepository,
        IInventoryService inventoryService)
    {
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
        _transactionRepository = transactionRepository;
        _inventoryService = inventoryService;
    }

    public async Task<Guid> Handle(AddMedicineBatchCommand request, CancellationToken cancellationToken)
    {
        // Validate medicine exists
        var medicine = await _medicineRepository.GetByIdAsync(request.MedicineId);
        if (medicine == null)
            throw new MedicineBatchNotFoundException($"Medicine with ID {request.MedicineId} not found.");

        // Validate expiry date
        if (request.ExpiryDate <= DateTime.UtcNow)
            throw new ExpiredMedicineException();

        // Validate initial stock
        if (request.QuantityAvailable <= 0)
            throw new InvalidStockAdjustmentException();

        // Create new batch entity
        var batch = new MedicineBatchEntity
        {
            Id = Guid.NewGuid(),
            MedicineId = request.MedicineId,
            BatchNumber = request.BatchNumber,
            QuantityAvailable = request.QuantityAvailable,
            PurchasePrice = request.PurchasePrice,
            SellingPrice = request.SellingPrice,
            ExpiryDate = request.ExpiryDate,
            SupplierId = request.SupplierId,
            ManufacturingDate = DateTime.UtcNow // Or from request if available
        };
        await _batchRepository.AddAsync(batch);

        // Update medicine total stock
        await _inventoryService.UpdateMedicineTotalStockAsync(request.MedicineId);

        // Create inventory transaction
        var transaction = new InventoryTransactionEntity
        {
            Id = Guid.NewGuid(),
            MedicineBatchId = batch.Id,
            MedicineId = request.MedicineId,
            Quantity = request.QuantityAvailable,
            TransactionDate = DateTime.UtcNow,
            ReferenceNumber = null,
            Remarks = "Batch added"
        };
        await _transactionRepository.AddAsync(transaction);

        return batch.Id;
    }
}
