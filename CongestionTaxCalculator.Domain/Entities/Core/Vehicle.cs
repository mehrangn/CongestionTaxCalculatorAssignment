namespace CongestionTaxCalculator.Domain.Entities.Core;

public class Vehicle
{
    public int Id { get; set; }
    public VehicleType Type { get; protected set; }
    public bool IsTollFree { get; protected set; }
}
