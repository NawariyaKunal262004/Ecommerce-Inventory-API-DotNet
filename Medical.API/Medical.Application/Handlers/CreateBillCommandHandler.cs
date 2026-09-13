namespace Medical.Application.Handlers;

public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, BillResponse>
{
    private readonly IBillRepository _billRepository;
    public CreateBillCommandHandler(IBillRepository billRepository) => _billRepository = billRepository;
    public async Task<BillResponse> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        var bill = new BillEntity
        {
            BillId = Guid.NewGuid(),
            PatientId = request.PatientId,
            TotalAmount = 0,
            CreatedDate = DateTime.UtcNow,
            Discount = 0,
            Tax = 0,
            FinalAmount = 0,
            BillItems = new List<BillItemEntity>()
        };
        var createdBill = await _billRepository.CreateBillAsync(bill);
        return new BillResponse
        {
            BillId = createdBill.BillId,
            CreatedDate = createdBill.CreatedDate,
            TotalAmount = createdBill.TotalAmount,
            Discount = createdBill.Discount,
            Tax = createdBill.Tax,
            FinalAmount = createdBill.FinalAmount,
            Items = new List<BillItemResponse>()
        };
    }
}
