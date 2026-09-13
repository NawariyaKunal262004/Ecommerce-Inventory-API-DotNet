namespace Medical.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AuthConstants.Admin)]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("batches")]
    public async Task<IActionResult> AddBatch([FromBody] AddMedicineBatchCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("batches/{id}")]
    public async Task<IActionResult> UpdateBatch(Guid id, [FromBody] UpdateMedicineBatchCommand command)
    {
        command.Id = id;
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("batches/{id}")]
    public async Task<IActionResult> DeleteBatch(Guid id)
        => Ok(await _mediator.Send(new DeleteMedicineBatchCommand { Id = id }));

    [HttpPost("adjust-stock")]
    public async Task<IActionResult> AdjustStock([FromBody] AdjustStockCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPost("deduct-stock")]
    public async Task<IActionResult> DeductStock([FromBody] DeductStockCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPost("restore-stock")]
    public async Task<IActionResult> RestoreStock([FromBody] RestoreStockCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPost("remove-expired")]
    public async Task<IActionResult> RemoveExpired([FromBody] RemoveExpiredStockCommand command)
        => Ok(await _mediator.Send(command));

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
        => Ok(await _mediator.Send(new GetInventorySummaryQuery()));

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
        => Ok(await _mediator.Send(new GetLowStockMedicinesQuery { Threshold = threshold }));

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiring()
        => Ok(await _mediator.Send(new GetExpiringMedicinesQuery()));

    [HttpGet("audit")]
    public async Task<IActionResult> GetAudit()
        => Ok(await _mediator.Send(new GetStockAuditQuery()));

    [HttpGet("batches/{medicineId}")]
    public async Task<IActionResult> GetBatches(Guid medicineId)
        => Ok(await _mediator.Send(new GetMedicineBatchesQuery { MedicineId = medicineId }));

    [HttpGet("batches")]
    public async Task<IActionResult> GetAllBatches()
        => Ok(await _mediator.Send(new GetInventoryBatchesQuery()));

    [HttpGet("batches/{id}/details")]
    public async Task<IActionResult> GetBatchById(Guid id)
        => Ok(await _mediator.Send(new GetMedicineBatchByIdQuery { BatchId = id }));

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions()
        => Ok(await _mediator.Send(new GetInventoryTransactionsQuery()));
}
