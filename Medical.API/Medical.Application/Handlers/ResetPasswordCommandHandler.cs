namespace Medical.Application.Handlers;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly IUserRepository _userRepository;

    public ResetPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.ResetPasswordAsync(request.UserId, request.NewPassword);
        if (!result.Success)
        {
            if (result.Message == CommonResource.UserNotFound)
                throw new UserNotFoundException();
            throw new InvalidOperationException(result.Message);
        }

        return new ResetPasswordResponse
        {
            Success = true,
            Message = result.Message ?? CommonResource.PasswordResetSuccessfully
        };
    }
}
