using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Application.Services;

public class VehicleFactory : IVehicleFactory
{
    public Vehicle? CreateVehicle(VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Car => new Car(),
            VehicleType.Motorbike => new Motorbike(),
            VehicleType.Emergency => new Emergency(),
            VehicleType.Bus => new Bus(),
            VehicleType.Diplomat => new Diplomat(),
            VehicleType.Foreign => new Foreign(),
            VehicleType.Military => new Military(),
            _ => null
        };
    }
}

