namespace Medical.Application.Commands;

public class ChangePasswordCommand : IRequest<ChangePasswordResponse>
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
