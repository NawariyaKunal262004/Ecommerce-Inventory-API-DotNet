namespace Medical.Application.Handlers;
public class GetBillByIdQueryHandler : IRequestHandler<GetBillByIdQuery, BillResponse>
{
    private readonly IBillRepository _billRepository;

    public GetBillByIdQueryHandler(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<BillResponse> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.GetBillByIdAsync(request.BillId);
        if (bill == null)
            throw new InvalidOperationException($"Bill with ID {request.BillId} not found.");

        return new BillResponse
        {
            BillId = bill.BillId,
            CreatedDate = bill.CreatedDate,
            TotalAmount = bill.TotalAmount,
            Discount = bill.Discount,
            Tax = bill.Tax,
            FinalAmount = bill.FinalAmount,
            Items = bill.BillItems.Select(bi => new BillItemResponse
            {
                BillItemId = bi.BillItemId,
                MedicineId = bi.MedicineId,
                MedicineName = bi.Medicine?.MedicineName ?? "Unknown",
                Quantity = bi.Quantity,
                UnitPrice = bi.UnitPrice,
                TotalPrice = bi.TotalPrice
            }).ToList()
        };
    }
}
