namespace Medical.Application.AI;
public class AIService : IAIService
{
    private readonly IMedicineRepository _medicineRepository;
    private readonly IBillRepository _billRepository;
    private readonly IMedicineBatchRepository _medicineBatchRepository;

    public AIService(
        IMedicineRepository medicineRepository,
        IBillRepository billRepository,
        IMedicineBatchRepository medicineBatchRepository)
    {
        _medicineRepository = medicineRepository;
        _billRepository = billRepository;
        _medicineBatchRepository = medicineBatchRepository;
    }

    public async Task<List<MedicineSuggestionResponse>> SuggestMedicinesAsync(Guid? patientId = null, CancellationToken cancellationToken = default)
    {
        var medicines = await _medicineRepository.GetAllAsync();
        var bills = await _billRepository.GetAllBillsWithItemsAsync(cancellationToken);

        var medicineUsage = bills
            .SelectMany(b => b.BillItems)
            .GroupBy(bi => bi.MedicineId)
            .Select(g => new
            {
                MedicineId = g.Key,
                TotalQuantity = g.Sum(bi => bi.Quantity),
                BillCount = g.Select(bi => bi.BillId).Distinct().Count()
            })
            .ToList();

        var suggestions = medicines.Select(m =>
        {
            var usage = medicineUsage.FirstOrDefault(u => u.MedicineId == m.MedicineId);
            var totalQuantity = usage?.TotalQuantity ?? 0;
            var billCount = usage?.BillCount ?? 0;

            var confidenceScore = 0m;
            var reason = "New medicine";

            if (totalQuantity > 0)
            {
                confidenceScore = Math.Min(100, (totalQuantity * 5) + (billCount * 10));
                reason = $"Frequently sold (sold {totalQuantity} units in {billCount} bills)";
            }

            return new MedicineSuggestionResponse
            {
                MedicineId = m.MedicineId,
                MedicineName = m.MedicineName ?? "Unknown",
                MedicineCategory = m.MedicineCategory,
                MedicinePrice = m.MedicinePrice,
                Stock = m.Stock,
                ConfidenceScore = confidenceScore,
                Reason = reason
            };
        })
        .OrderByDescending(s => s.ConfidenceScore)
        .Take(10)
        .ToList();

        return suggestions;
    }

    public async Task<List<StockPredictionResponse>> GetLowStockPredictionAsync(CancellationToken cancellationToken = default)
    {
        var medicines = await _medicineRepository.GetAllAsync();
        var bills = await _billRepository.GetAllBillsWithItemsAsync(cancellationToken);

        var thirtyDaysAgo = DateTime.Now.AddDays(-30);
        var recentBills = bills.Where(b => b.CreatedDate >= thirtyDaysAgo).ToList();

        var predictions = medicines.Select(m =>
        {
            var recentUsage = recentBills
                .SelectMany(b => b.BillItems)
                .Where(bi => bi.MedicineId == m.MedicineId)
                .Sum(bi => bi.Quantity);

            var averageDailyUsage = recentUsage / 30m;
            var estimatedDaysRemaining = averageDailyUsage > 0 ? (int)(m.Stock / averageDailyUsage) : int.MaxValue;

            string riskLevel;
            string suggestion;

            if (estimatedDaysRemaining <= 7)
            {
                riskLevel = "Critical";
                suggestion = "Reorder immediately!";
            }
            else if (estimatedDaysRemaining <= 30)
            {
                riskLevel = "High";
                suggestion = "Consider reordering soon";
            }
            else if (estimatedDaysRemaining <= 60)
            {
                riskLevel = "Medium";
                suggestion = "Monitor stock levels";
            }
            else
            {
                riskLevel = "Low";
                suggestion = "Stock is sufficient";
            }

            return new StockPredictionResponse
            {
                MedicineId = m.MedicineId,
                MedicineName = m.MedicineName ?? "Unknown",
                CurrentStock = m.Stock,
                AverageDailyUsage = Math.Round(averageDailyUsage, 2),
                EstimatedDaysRemaining = estimatedDaysRemaining,
                RiskLevel = riskLevel,
                Suggestion = suggestion
            };
        })
        .OrderBy(p => p.EstimatedDaysRemaining)
        .ToList();

        return predictions;
    }

    public async Task<SalesTrendAnalysisResponse> AnalyzeSalesTrendsAsync(int days = 30, CancellationToken cancellationToken = default)
    {
        var bills = await _billRepository.GetAllBillsWithItemsAsync(cancellationToken);
        var startDate = DateTime.Now.AddDays(-days);
        var startDate7Days = DateTime.Now.AddDays(-7);

        var relevantBills = bills.Where(b => b.CreatedDate >= startDate).ToList();
        var last7DaysBills = bills.Where(b => b.CreatedDate >= startDate7Days).ToList();

        var dailySales = relevantBills
            .GroupBy(b => b.CreatedDate.Date)
            .Select(g => new DailySalesItem
            {
                Date = g.Key,
                TotalSales = g.Sum(b => b.FinalAmount)
            })
            .OrderBy(d => d.Date)
            .ToList();

        var topSellingMedicines = relevantBills
            .SelectMany(b => b.BillItems)
            .GroupBy(bi => bi.MedicineId)
            .Select(g => new
            {
                MedicineId = g.Key,
                MedicineName = g.First().Medicine?.MedicineName ?? "Unknown",
                TotalQuantitySold = g.Sum(bi => bi.Quantity),
                TotalRevenue = g.Sum(bi => bi.TotalPrice)
            })
            .OrderByDescending(x => x.TotalQuantitySold)
            .Take(5)
            .Select(x => new TopSellingMedicine
            {
                MedicineId = x.MedicineId,
                MedicineName = x.MedicineName,
                TotalQuantitySold = x.TotalQuantitySold,
                TotalRevenue = x.TotalRevenue
            })
            .ToList();

        var totalRevenueLast7Days = last7DaysBills.Sum(b => b.FinalAmount);
        var totalRevenueLast30Days = relevantBills.Sum(b => b.FinalAmount);

        var previous7DaysStart = DateTime.Now.AddDays(-14);
        var previous7DaysBills = bills.Where(b => b.CreatedDate >= previous7DaysStart && b.CreatedDate < startDate7Days).ToList();
        var previous7DaysRevenue = previous7DaysBills.Sum(b => b.FinalAmount);

        var revenueGrowthPercentage = previous7DaysRevenue > 0
            ? Math.Round(((totalRevenueLast7Days - previous7DaysRevenue) / previous7DaysRevenue) * 100, 2)
            : totalRevenueLast7Days > 0 ? 100 : 0;

        return new SalesTrendAnalysisResponse
        {
            TotalRevenueLast7Days = totalRevenueLast7Days,
            TotalRevenueLast30Days = totalRevenueLast30Days,
            RevenueGrowthPercentage = revenueGrowthPercentage,
            DailySales = dailySales,
            TopSellingMedicines = topSellingMedicines
        };
    }

    public async Task<BillTotalCalculationResponse> AutoCalculateBillTotalAsync(List<AddBillItemRequest> items, decimal? discount = null, decimal? taxRate = null, CancellationToken cancellationToken = default)
    {
        var medicines = await _medicineRepository.GetAllAsync();
        var response = new BillTotalCalculationResponse();
        var warnings = new List<string>();
        var appliedDiscounts = new List<string>();

        var subtotal = 0m;
        var itemCounts = new Dictionary<Guid, int>();

        foreach (var item in items)
        {
            var medicine = medicines.FirstOrDefault(m => m.MedicineId == item.MedicineId);
            if (medicine == null)
            {
                warnings.Add($"Medicine with ID {item.MedicineId} not found");
                continue;
            }

            var unitPrice = medicine.MedicinePrice;
            var itemTotal = unitPrice * item.Quantity;
            subtotal += itemTotal;

            if (itemCounts.ContainsKey(item.MedicineId))
            {
                itemCounts[item.MedicineId] += item.Quantity;
                warnings.Add($"Duplicate medicine {medicine.MedicineName} in bill items");
            }
            else
            {
                itemCounts[item.MedicineId] = item.Quantity;
            }
        }

        var totalQuantity = items.Sum(i => i.Quantity);
        decimal calculatedDiscount = discount ?? 0;

        if (totalQuantity >= 10 && discount == null)
        {
            calculatedDiscount = subtotal * 0.05m;
            appliedDiscounts.Add("Bulk purchase discount (5%)");
        }

        var tax = taxRate.HasValue ? subtotal * taxRate.Value : subtotal * 0.08m;
        if (!taxRate.HasValue)
        {
            appliedDiscounts.Add("Standard tax rate (8%)");
        }

        var finalTotal = subtotal - calculatedDiscount + tax;

        response.Subtotal = Math.Round(subtotal, 2);
        response.Discount = Math.Round(calculatedDiscount, 2);
        response.Tax = Math.Round(tax, 2);
        response.FinalTotal = Math.Round(finalTotal, 2);
        response.AppliedDiscounts = appliedDiscounts;
        response.Warnings = warnings;

        return response;
    }
}
