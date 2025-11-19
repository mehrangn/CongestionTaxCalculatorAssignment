using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Motorbike : Vehicle
{
    public Motorbike()
    {
        Type = VehicleType.Motorbike;
        IsTollFree = true;
    }
}

