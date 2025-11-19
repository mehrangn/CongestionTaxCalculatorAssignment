namespace CongestionTaxCalculator.Application.Common.City.Requests;

public class TaxRuleRequest
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal Amount { get; set; }
}

