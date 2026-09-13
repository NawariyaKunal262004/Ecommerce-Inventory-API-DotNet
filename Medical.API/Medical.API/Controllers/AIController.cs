namespace Medical.API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AuthConstants.Admin)]
public class AIController : ControllerBase
{
    private readonly IMediator _mediator;

    public AIController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("suggest-medicines")]
    public async Task<IActionResult> SuggestMedicines([FromQuery] Guid? patientId = null)
    {
        var query = new SuggestMedicinesQuery { PatientId = patientId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("low-stock-prediction")]
    public async Task<IActionResult> GetLowStockPrediction()
    {
        var query = new GetLowStockPredictionQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("sales-trends")]
    public async Task<IActionResult> AnalyzeSalesTrends([FromQuery] int days = 30)
    {
        var query = new AnalyzeSalesTrendsQuery { Days = days };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("calculate-bill-total")]
    public async Task<IActionResult> AutoCalculateBillTotal([FromBody] AutoCalculateBillTotalQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
