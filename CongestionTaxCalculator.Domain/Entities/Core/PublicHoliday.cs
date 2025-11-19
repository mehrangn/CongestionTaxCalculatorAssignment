namespace CongestionTaxCalculator.Domain.Entities;

public class PublicHoliday
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDayBeforeHoliday { get; set; }
}

