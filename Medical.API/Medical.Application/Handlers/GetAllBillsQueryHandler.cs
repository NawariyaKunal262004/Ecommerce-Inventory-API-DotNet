namespace Medical.Application.Handlers;
public class GetAllBillsQueryHandler : IRequestHandler<GetAllBillsQuery, List<BillResponse>>
{
    private readonly IBillRepository _billRepository;

    public GetAllBillsQueryHandler(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<List<BillResponse>> Handle(GetAllBillsQuery request, CancellationToken cancellationToken)
    {
        var bills = await _billRepository.GetAllBillsWithItemsAsync(cancellationToken);
        return bills.Select(MapToResponse).ToList();
    }

    private BillResponse MapToResponse(BillEntity bill)
    {
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
