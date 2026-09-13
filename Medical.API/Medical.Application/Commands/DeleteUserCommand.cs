namespace Medical.Application.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    public string UserId { get; set; } = string.Empty;
}
