using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Military : Vehicle
{
    public Military()
    {
        Type = VehicleType.Military;
        IsTollFree = true;
    }
}

