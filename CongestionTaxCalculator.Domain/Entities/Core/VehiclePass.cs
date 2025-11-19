using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class VehiclePass
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public DateTime PassTime { get; set; }
    public decimal TaxAmount { get; set; }
}

