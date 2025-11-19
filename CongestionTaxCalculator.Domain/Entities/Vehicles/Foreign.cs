using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Foreign : Vehicle
{
    public Foreign()
    {
        Type = VehicleType.Foreign;
        IsTollFree = true;
    }
}

