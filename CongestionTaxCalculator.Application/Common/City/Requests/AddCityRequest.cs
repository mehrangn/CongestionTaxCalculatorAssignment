namespace CongestionTaxCalculator.Application.Common.City.Requests;

public class AddCityRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal MaxDailyTax { get; set; }
    public int SingleChargeMinutes { get; set; } = 60;
    public bool IsActive { get; set; } = true;
    public IList<TaxRuleRequest> TaxRules { get; set; } = new List<TaxRuleRequest>();
}
