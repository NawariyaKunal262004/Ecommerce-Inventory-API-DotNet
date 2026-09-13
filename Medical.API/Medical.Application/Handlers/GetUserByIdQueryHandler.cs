namespace Medical.Application.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetUserByIdAsync(request.UserId);
        if (!result.Success || result.Data == null)
            throw new UserNotFoundException(result.Message);

        return result.Data is UserDto dto
            ? UserMappingHelper.MapFromDto(dto)
            : UserMappingHelper.MapFromAnonymous(result.Data);
    }
}
