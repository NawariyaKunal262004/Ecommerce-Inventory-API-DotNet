namespace Medical.Application.Commands;

public class ResetPasswordCommand : IRequest<ResetPasswordResponse>
{
    public string UserId { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
