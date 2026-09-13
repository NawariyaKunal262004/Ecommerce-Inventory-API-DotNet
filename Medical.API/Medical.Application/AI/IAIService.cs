namespace Medical.Application.AI;
public interface IAIService
{
    Task<List<MedicineSuggestionResponse>> SuggestMedicinesAsync(Guid? patientId = null, CancellationToken cancellationToken = default);
    Task<List<StockPredictionResponse>> GetLowStockPredictionAsync(CancellationToken cancellationToken = default);
    Task<SalesTrendAnalysisResponse> AnalyzeSalesTrendsAsync(int days = 30, CancellationToken cancellationToken = default);
    Task<BillTotalCalculationResponse> AutoCalculateBillTotalAsync(List<AddBillItemRequest> items, decimal? discount = null, decimal? taxRate = null, CancellationToken cancellationToken = default);
}
