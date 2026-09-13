namespace Medical.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateUserCommandHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var organizationId = request.OrganizationId;
        if (organizationId == Guid.Empty && _currentUserService.OrganizationId.HasValue)
            organizationId = _currentUserService.OrganizationId.Value;

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role,
            OrganizationId = organizationId,
            EmailConfirmed = true
        };

        var result = await _userRepository.CreateAsync(user, request.Password);
        if (!result.Success)
        {
            if (result.Message == CommonResource.InvalidRole)
                throw new RoleAssignmentException(result.Message);
            throw new InvalidOperationException(result.Message);
        }

        return result.Data is UserDto dto
            ? UserMappingHelper.MapFromDto(dto)
            : UserMappingHelper.MapFromAnonymous(result.Data!);
    }
}
