namespace Medical.API.Controllers;
[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = AuthConstants.Admin)]
public class SupplierController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplierController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> CreateSupplier([FromBody] CreateSupplierCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetSupplierById), new { id = result.SupplierId }, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplierResponse>>> GetSuppliers()
    {
        var result = await _mediator.Send(new GetSuppliersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierResponse>> GetSupplierById(Guid id)
    {
        var result = await _mediator.Send(new GetSupplierQuery(id));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SupplierResponse>> UpdateSupplier(Guid id, [FromBody] UpdateSupplierCommand command)
    {
        command.SupplierId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteSupplier(Guid id)
    {
        var command = new DeleteSupplierCommand { SupplierId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
