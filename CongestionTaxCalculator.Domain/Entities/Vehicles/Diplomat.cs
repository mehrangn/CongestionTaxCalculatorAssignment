using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Diplomat : Vehicle
{
    public Diplomat()
    {
        Type = VehicleType.Diplomat;
        IsTollFree = true;
    }
}

