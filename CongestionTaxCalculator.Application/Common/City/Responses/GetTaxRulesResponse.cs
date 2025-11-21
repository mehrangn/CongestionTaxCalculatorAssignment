namespace CongestionTaxCalculator.Application.Common.City.Responses;
using CongestionTaxCalculator.Application.Common.City.Dtos;

public class GetTaxRulesResponse
{
    public string CityName { get; set; } = string.Empty;
    public decimal MaxDailyTax { get; set; }
    public int SingleChargeMinutes { get; set; }
    public ICollection<TaxRuleDto> TaxRules { get; set; } = new List<TaxRuleDto>();
}

