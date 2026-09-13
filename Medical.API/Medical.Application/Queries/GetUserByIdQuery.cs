namespace Medical.Application.Queries;

public class GetUserByIdQuery : IRequest<UserResponse>
{
    public string UserId { get; set; } = string.Empty;
}
