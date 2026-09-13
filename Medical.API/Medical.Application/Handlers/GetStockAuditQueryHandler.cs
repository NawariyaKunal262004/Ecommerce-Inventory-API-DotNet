namespace Medical.Application.Handlers;
public class GetStockAuditQueryHandler : IRequestHandler<GetStockAuditQuery, ResponseModel>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryTransactionRepository _transactionRepository;
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;

    public GetStockAuditQueryHandler(
        IInventoryRepository inventoryRepository,
        IInventoryTransactionRepository transactionRepository,
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository)
    {
        _inventoryRepository = inventoryRepository;
        _transactionRepository = transactionRepository;
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
    }

    public async Task<ResponseModel> Handle(GetStockAuditQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _inventoryRepository.GetInventoryTransactionsAsync(request.MedicineId);
        var medicines = await _medicineRepository.GetAllAsync();
        var medicineDict = medicines.ToDictionary(m => m.MedicineId, m => m.MedicineName);
        var batches = await _batchRepository.GetBatchesByMedicineIdAsync(request.MedicineId ?? Guid.Empty);
        var batchDict = batches.ToDictionary(b => b.Id, b => b.BatchNumber);

        var transactionResponses = transactions.Select(t => new InventoryTransactionResponse
        {
            Id = t.Id,
            MedicineBatchId = t.MedicineBatchId,
            MedicineId = t.MedicineId,
            MedicineName = medicineDict.ContainsKey(t.MedicineId) ? medicineDict[t.MedicineId] : "Unknown",
            Quantity = t.Quantity,
            TransactionDate = t.TransactionDate,
            ReferenceNumber = t.ReferenceNumber,
            Remarks = t.Remarks
        }).ToList();

        return ResponseModel.SuccessResponse(transactionResponses, 
            $"Retrieved {transactionResponses.Count} inventory transactions");
    }
}
