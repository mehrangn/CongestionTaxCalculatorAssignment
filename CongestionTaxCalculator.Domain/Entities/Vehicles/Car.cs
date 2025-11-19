using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Entities;

public class Car : Vehicle
{
    public Car()
    {
        Type = VehicleType.Car;
        IsTollFree = false;
    }
}

