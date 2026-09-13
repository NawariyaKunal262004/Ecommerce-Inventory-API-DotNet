namespace Medical.Application.Handlers;

    public class UpdateMedicineBatchCommandHandler : IRequestHandler<UpdateMedicineBatchCommand, ResponseModel>
    {
        private readonly IMedicineBatchRepository _batchRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IInventoryService _inventoryService;

        public UpdateMedicineBatchCommandHandler(
            IMedicineBatchRepository batchRepository,
            IMedicineRepository medicineRepository,
            IInventoryService inventoryService)
        {
            _batchRepository = batchRepository;
            _medicineRepository = medicineRepository;
            _inventoryService = inventoryService;
        }

        public async Task<ResponseModel> Handle(UpdateMedicineBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.Id);
            if (batch == null)
                throw new MedicineBatchNotFoundException();

            var medicine = await _medicineRepository.GetByIdAsync(batch.MedicineId);

            batch.BatchNumber = request.BatchNumber;
            batch.QuantityAvailable = request.QuantityAvailable;
            batch.PurchasePrice = request.PurchasePrice;
            batch.SellingPrice = request.SellingPrice;
            batch.ExpiryDate = request.ExpiryDate;
            batch.SupplierId = request.SupplierId;

            await _batchRepository.UpdateAsync(batch);
            await _inventoryService.UpdateMedicineTotalStockAsync(batch.MedicineId);

            var response = new MedicineBatchResponse
            {
                Id = batch.Id,
                MedicineId = batch.MedicineId,
                MedicineName = medicine?.MedicineName,
                BatchNumber = batch.BatchNumber,
                QuantityAvailable = batch.QuantityAvailable,
                PurchasePrice = batch.PurchasePrice,
                SellingPrice = batch.SellingPrice,
                ManufacturingDate = batch.ManufacturingDate,
                ExpiryDate = batch.ExpiryDate,
                SupplierId = batch.SupplierId
            };

            return ResponseModel.SuccessResponse(response, 
                $"Successfully updated batch {batch.BatchNumber} of medicine {medicine?.MedicineName ?? "Unknown"}");
        }
    }

