namespace Medical.Application.Exceptions;
public class ExpiredMedicineException : Exception
{
    public ExpiredMedicineException(string? message = null)
        : base(message ?? "Medicine batch is expired.")
    {
    }
}
