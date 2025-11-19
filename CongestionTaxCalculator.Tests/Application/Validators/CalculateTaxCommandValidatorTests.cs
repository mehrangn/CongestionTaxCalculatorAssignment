using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CongestionTaxCalculator.Application.TaxHandlers.CalculateTax;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.UnitTests.Application.Validators;
[TestClass]
public class CalculateTaxCommandValidatorTests
{
    private readonly CalculateTaxCommandValidator _validator;

    public CalculateTaxCommandValidatorTests()
    {
        _validator = new CalculateTaxCommandValidator();
    }

    [TestMethod]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) },
            CityCode = "GOT"
        };
        
        var result = _validator.Validate(command);
        
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Validate_WithNullDates_ShouldFail()
    {
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = null!,
            CityCode = "GOT"
        };
        
        var result = _validator.Validate(command);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dates");
    }

    [TestMethod]
    public void Validate_WithEmptyDates_ShouldFail()
    {
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = Array.Empty<DateTime>(),
            CityCode = "GOT"
        };
        
        var result = _validator.Validate(command);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dates");
    }

    [TestMethod]
    public void Validate_WithEmptyCityCode_ShouldFail()
    {
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) },
            CityCode = string.Empty
        };
        
        var result = _validator.Validate(command);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CityCode");
    }

    [TestMethod]
    public void Validate_WithDateNotIn2013_ShouldFail()
    {
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = new[] { new DateTime(2014, 2, 8, 6, 0, 0) },
            CityCode = "GOT"
        };
        
        var result = _validator.Validate(command);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Dates[0]");
    }

    [TestMethod]
    public void Validate_WithCityCodeTooLong_ShouldFail()
    {
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) },
            CityCode = new string('A', 11)
        };
        
        var result = _validator.Validate(command);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CityCode");
    }
}
