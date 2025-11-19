namespace CongestionTaxCalculator.Application.Common.City.Responses;

public class CityResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal MaxDailyTax { get; set; }
    public int SingleChargeMinutes { get; set; }
    public bool IsActive { get; set; }
}

