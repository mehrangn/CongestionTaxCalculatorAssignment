using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Emergency : Vehicle
{
    public Emergency()
    {
        Type = VehicleType.Emergency;
        IsTollFree = true;
    }
}

