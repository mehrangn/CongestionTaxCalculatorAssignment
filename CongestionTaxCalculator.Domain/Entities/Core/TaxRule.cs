namespace CongestionTaxCalculator.Domain.Entities;

public class TaxRule
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal Amount { get; set; }
}

