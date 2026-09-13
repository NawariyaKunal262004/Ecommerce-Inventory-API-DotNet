namespace Medical.Application.Handlers;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentUserQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
            throw new AuthUnauthorizedException();

        var result = await _userRepository.GetUserByIdAsync(_currentUserService.UserId);
        if (!result.Success || result.Data == null)
            throw new UserNotFoundException(result.Message);

        return result.Data is UserDto dto
            ? UserMappingHelper.MapFromDto(dto)
            : UserMappingHelper.MapFromAnonymous(result.Data);
    }
}
