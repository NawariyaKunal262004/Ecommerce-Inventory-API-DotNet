namespace Medical.Application.Queries;
public class GetLowStockMedicinesQuery : IRequest<ResponseModel>
{
    public int Threshold { get; set; } = 10;
}
