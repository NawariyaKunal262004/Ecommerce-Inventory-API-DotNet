namespace Medical.Application.Exceptions;
public class InvalidBillOperationException : Exception
{
    public InvalidBillOperationException(string message)
        : base(message)
    {
    }
}
