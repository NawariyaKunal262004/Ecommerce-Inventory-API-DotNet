namespace Medical.Application.Queries;
public class GetDailySalesQuery : IRequest<decimal>
{
    public DateTime Date { get; set; }
}
