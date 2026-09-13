namespace Medical.Application.Exceptions;

public class AuthUnauthorizedException : Exception
{
    public AuthUnauthorizedException(string? message = null)
        : base(message ?? "You are not authorized to perform this action.")
    {
    }
}
