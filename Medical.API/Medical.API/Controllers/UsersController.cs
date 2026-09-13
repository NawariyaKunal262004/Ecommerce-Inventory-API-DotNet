namespace Medical.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = AuthConstants.Admin)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IList<UserResponse>>> GetUsers()
    {
        var result = await _mediator.Send(new GetUsersQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUserById(string id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery { UserId = id });
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> UpdateUser(string id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(string id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { UserId = id });
        return Ok(result);
    }
}
