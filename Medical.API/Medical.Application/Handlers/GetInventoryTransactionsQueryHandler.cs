namespace Medical.Application.Handlers;
public class GetInventoryTransactionsQueryHandler : IRequestHandler<GetInventoryTransactionsQuery, ResponseModel>
{
    private readonly IInventoryTransactionRepository _transactionRepository;
    private readonly IMedicineBatchRepository _batchRepository;
    private readonly IMedicineRepository _medicineRepository;

    public GetInventoryTransactionsQueryHandler(
        IInventoryTransactionRepository transactionRepository,
        IMedicineBatchRepository batchRepository,
        IMedicineRepository medicineRepository)
    {
        _transactionRepository = transactionRepository;
        _batchRepository = batchRepository;
        _medicineRepository = medicineRepository;
    }

    public async Task<ResponseModel> Handle(GetInventoryTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var medicines = await _medicineRepository.GetAllAsync();
        var medicineDict = medicines.ToDictionary(m => m.MedicineId, m => m.MedicineName);

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
