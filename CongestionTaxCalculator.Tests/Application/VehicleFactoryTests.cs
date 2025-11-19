using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CongestionTaxCalculator.Application.Services;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.UnitTests.Application;

[TestClass]
public class VehicleFactoryTests
{
    private readonly IVehicleFactory _vehicleFactory;

    public VehicleFactoryTests()
    {
        _vehicleFactory = new VehicleFactory();
    }

    [TestMethod]
    [DataRow(VehicleType.Car, typeof(Car))]
    [DataRow(VehicleType.Motorbike, typeof(Motorbike))]
    [DataRow(VehicleType.Emergency, typeof(Emergency))]
    [DataRow(VehicleType.Bus, typeof(Bus))]
    [DataRow(VehicleType.Diplomat, typeof(Diplomat))]
    [DataRow(VehicleType.Foreign, typeof(Foreign))]
    [DataRow(VehicleType.Military, typeof(Military))]
    public void CreateVehicle_WithValidType_ShouldReturnCorrectVehicle(VehicleType vehicleType, Type expectedType)
    {
        var result = _vehicleFactory.CreateVehicle(vehicleType);

        result.Should().NotBeNull();
        result.Should().BeOfType(expectedType);
    }

    [TestMethod]
    public void CreateVehicle_WithInvalidType_ShouldReturnNull()
    {
        var invalidType = (VehicleType)999;
        var result = _vehicleFactory.CreateVehicle(invalidType);

        result.Should().BeNull();
    }
}
