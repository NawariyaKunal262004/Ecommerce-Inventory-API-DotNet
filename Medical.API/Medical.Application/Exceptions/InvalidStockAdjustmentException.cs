namespace Medical.Application.Exceptions;
public class InvalidStockAdjustmentException : Exception
{
    public InvalidStockAdjustmentException(string? message = null)
        : base(message ?? "Invalid stock adjustment.")
    {
    }
}
