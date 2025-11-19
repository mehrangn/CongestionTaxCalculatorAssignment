namespace CongestionTaxCalculator.Application.Common.City.Dtos;

public class TaxRuleDto
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal Amount { get; set; }
}

