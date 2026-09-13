namespace Medical.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
public class BillsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ExportService _exportService;
    public BillsController(IMediator mediator, ExportService exportService)
    {
        _mediator = mediator;
        _exportService = exportService;
    }

    [HttpPost]
    public async Task<ActionResult<BillResponse>> CreateBill([FromBody] CreateBillCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBillById), new { id = result.BillId }, result);
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult<BillResponse>> AddBillItem([FromRoute] Guid id, [FromBody] AddBillItemRequest request)
    {
        var command = new AddBillCommand
        {
            BillId = id,
            MedicineId = request.MedicineId,
            Quantity = request.Quantity
        };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BillResponse>> GetBillById(Guid id)
    {
        var query = new GetBillByIdQuery { BillId = id };  // Now using Guid
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<BillResponse>>> GetAllBills()
    {
        var query = new GetAllBillsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("daily-sales")]
    [Authorize(Roles = AuthConstants.Admin)]
    public async Task<ActionResult<object>> GetDailySales([FromQuery] DateTime? date = null)
    {
        var salesDate = date ?? DateTime.UtcNow;
        var query = new GetDailySalesQuery { Date = salesDate };
        var totalSales = await _mediator.Send(query);
        return Ok(new { date = salesDate.Date, totalSales });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BillResponse>> UpdateBill(Guid id, [FromBody] UpdateBillCommand command)
    {
        command.BillId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteBill(Guid id)
    {
        var command = new DeleteBillCommand { BillId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("items/{id}")]
    public async Task<ActionResult<BillResponse>> UpdateBillItem(Guid id, [FromBody] UpdateBillItemCommand command)
    {
        command.BillItemId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("items/{id}")]
    public async Task<ActionResult<BillResponse>> DeleteBillItem(Guid id)
    {
        var command = new DeleteBillItemCommand { BillItemId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportBills([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var query = new GetAllBillsQuery();
        var bills = await _mediator.Send(query);

        var csvContent = _exportService.ExportToCsv(bills);
        var csvBytes = _exportService.GetCsvBytes(csvContent);

        return File(csvBytes, "text/csv", $"bills_{DateTime.UtcNow:yyyyMMdd}.csv");
    }
}
