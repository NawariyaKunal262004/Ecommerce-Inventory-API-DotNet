namespace Medical.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{AuthConstants.Admin},{AuthConstants.Biller}")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> CreatePatient([FromBody] CreatePatientCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPatientById), new { id = result.PatientId }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResponse>> GetPatientById(Guid id)
    {
        try
        {
            var query = new GetPatientByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<PatientHistoryResponse>> GetPatientHistory(Guid id)
    {
        try
        {
            var query = new GetPatientHistoryQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientResponse>>> GetAllPatients()
    {
        var query = new GetAllPatientsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientResponse>> UpdatePatient(Guid id, [FromBody] UpdatePatientCommand command)
    {
        command.PatientId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeletePatient(Guid id)
    {
        var command = new DeletePatientCommand { PatientId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
