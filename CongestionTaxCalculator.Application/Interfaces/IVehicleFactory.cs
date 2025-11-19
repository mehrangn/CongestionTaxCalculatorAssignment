using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Application.Interfaces;

public interface IVehicleFactory
{
    Vehicle? CreateVehicle(VehicleType vehicleType);
}

