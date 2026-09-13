namespace Medical.Application.Handlers;
public class GetPatientHistoryQueryHandler : IRequestHandler<GetPatientHistoryQuery, PatientHistoryResponse>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IBillRepository _billRepository;

    public GetPatientHistoryQueryHandler(
        IPatientRepository patientRepository,
        IBillRepository billRepository)
    {
        _patientRepository = patientRepository;
        _billRepository = billRepository;
    }

    public async Task<PatientHistoryResponse> Handle(GetPatientHistoryQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Fetch patient details
        var patient = await _patientRepository.GetByIdAsync(request.PatientId);
        if (patient == null)
            throw new InvalidOperationException($"Patient with ID {request.PatientId} not found.");

        // Step 2: Fetch patient's bills (integration with Billing Module)
        var bills = await _patientRepository.GetPatientBillsAsync(request.PatientId);

        // Step 3: Calculate age
        var age = CalculateAge(patient.DateOfBirth);

        // Step 4: Map bills to response DTOs
        var billHistoryItems = new List<PatientBillHistoryItem>();
        decimal totalAmountSpent = 0;

        foreach (var bill in bills)
        {
            var billItems = new List<PatientBillItemDetail>();

            if (bill.BillItems != null && bill.BillItems.Any())
            {
                foreach (var item in bill.BillItems)
                {
                    billItems.Add(new PatientBillItemDetail
                    {
                        MedicineName = item.Medicine?.MedicineName ?? "Unknown Medicine",
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    });
                }
            }

            var billHistory = new PatientBillHistoryItem
            {
                BillId = bill.BillId,
                TotalAmount = bill.TotalAmount,
                Discount = bill.Discount,
                Tax = bill.Tax,
                FinalAmount = bill.FinalAmount,
                CreatedDate = bill.CreatedDate,
                ItemCount = billItems.Count,
                Items = billItems
            };

            billHistoryItems.Add(billHistory);
            totalAmountSpent += bill.FinalAmount;
        }

        // Step 5: Build response
        var response = new PatientHistoryResponse
        {
            PatientId = patient.PatientId,
            PatientName = patient.PatientName,
            Age = age,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            TotalBills = billHistoryItems.Count,
            TotalAmountSpent = totalAmountSpent,
            Bills = billHistoryItems
        };

        return response;
    }

    private int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}
