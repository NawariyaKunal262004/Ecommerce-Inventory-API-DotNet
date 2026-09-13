namespace Medical.Application.Handlers;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
            throw new AuthUnauthorizedException();

        var result = await _userRepository.ChangePasswordAsync(
            _currentUserService.UserId,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Success)
            throw new InvalidOperationException(result.Message);

        return new ChangePasswordResponse
        {
            Success = true,
            Message = result.Message ?? CommonResource.PasswordChangedSuccessfully
        };
    }
}
