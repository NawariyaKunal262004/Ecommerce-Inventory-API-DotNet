namespace Medical.Application.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Id = request.Id,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            OrganizationId = request.OrganizationId ?? Guid.Empty
        };

        var result = await _userRepository.UpdateAsync(user);
        if (!result.Success)
        {
            if (result.Message == CommonResource.UserNotFound)
                throw new UserNotFoundException();
            if (result.Message == CommonResource.InvalidRole)
                throw new RoleAssignmentException(result.Message);
            throw new InvalidOperationException(result.Message);
        }

        return result.Data is UserDto dto
            ? UserMappingHelper.MapFromDto(dto)
            : UserMappingHelper.MapFromAnonymous(result.Data!);
    }
}
