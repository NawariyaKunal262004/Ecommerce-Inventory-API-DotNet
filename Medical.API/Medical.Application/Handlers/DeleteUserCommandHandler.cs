namespace Medical.Application.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.DeleteAsync(request.UserId);
        if (!result.Success)
            throw new UserNotFoundException(result.Message);

        return true;
    }
}
