namespace Medical.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string? message = null)
        : base(message ?? "User not found.")
    {
    }
}
