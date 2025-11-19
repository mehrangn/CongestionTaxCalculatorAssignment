namespace CongestionTaxCalculator.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal MaxDailyTax { get; set; }
    public int SingleChargeMinutes { get; set; } = 60;
    public bool IsActive { get; set; } = true;
}

