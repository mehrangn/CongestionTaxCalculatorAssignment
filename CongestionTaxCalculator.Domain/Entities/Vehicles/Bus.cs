using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Bus : Vehicle
{
    public Bus()
    {
        Type = VehicleType.Bus;
        IsTollFree = true;
    }
}

