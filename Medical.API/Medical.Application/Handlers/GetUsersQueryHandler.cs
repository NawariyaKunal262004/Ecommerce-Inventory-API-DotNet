namespace Medical.Application.Handlers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IList<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUsersQueryHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IList<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.OrganizationId == null)
            throw new AuthUnauthorizedException();

        var result = await _userRepository.GetUsersAsync(_currentUserService.OrganizationId.Value);
        if (!result.Success || result.Data == null)
            return [];

        if (result.Data is List<UserDto> users)
            return users.Select(UserMappingHelper.MapFromDto).ToList();

        return UserMappingHelper.MapListFromAnonymous(result.Data);
    }
}
