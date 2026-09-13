namespace Medical.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = AuthConstants.Admin)]
    public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineCommand command)
    {
        // Send command to MediatR, which routes to CreateMedicineCommandHandler
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = AuthConstants.Admin)]
    public async Task<IActionResult> UpdateMedicine(Guid id, [FromBody] UpdateMedicineCommand command)
    {
        // Set the medicine ID from the URL parameter
        command.MedicineId = id;
        // Send command to MediatR, which routes to UpdateMedicineCommandHandler
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = AuthConstants.Admin)]
    public async Task<IActionResult> DeleteMedicine(Guid id)
    {
        // Create delete command with the ID from URL
        var command = new DeleteMedicineCommand { MedicineId = id };
        // Send command to MediatR, which routes to DeleteMedicineCommandHandler
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
    public async Task<IActionResult> GetAllMedicines()
    {
        // Create query object (no parameters)
        var query = new GetAllMedicineListQuery();
        // Send query to MediatR, which routes to GetAllMedicineListQueryHandler
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
    public async Task<IActionResult> GetMedicineById(Guid id)
    {
        // Create query with the ID from URL
        var query = new GetMedicineById(id);
        // Send query to MediatR, which routes to GetMedicineByIdQueryHandler
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("low-stock")]
    [Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
    public async Task<IActionResult> GetLowStockMedicines()
    {
        // Create query object (no parameters)
        var query = new GetLowStockQuery();
        // Send query to MediatR, which routes to GetLowStockQueryHandler
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("expiring")]
    [Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
    public async Task<IActionResult> GetExpiringMedicines()
    {
        // Create query object (no parameters)
        var query = new GetExpiringQuery();
        // Send query to MediatR, which routes to GetExpiringQueryHandler
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
