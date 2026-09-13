namespace Medical.Application.Commands;

public class RevokeTokenCommand : IRequest<bool>
{
    public string RefreshToken { get; set; } = string.Empty;
}
