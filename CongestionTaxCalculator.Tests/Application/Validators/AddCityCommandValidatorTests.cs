using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CongestionTaxCalculator.Application.Common.City.Dtos;
using CongestionTaxCalculator.Application.CityHandlers.AddCity;

namespace CongestionTaxCalculator.Tests.Application.Validators;

[TestClass]
public class AddCityCommandValidatorTests
{
    private readonly AddCityCommandValidator _validator;

    public AddCityCommandValidatorTests()
    {
        _validator = new AddCityCommandValidator();
    }

    [TestMethod]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var command = new AddCityCommand
        {
            Name = "Valid City",
            Code = "VAL",
            MaxDailyTax = 60,
            TaxRules = new List<TaxRuleDto>
            {
                new TaxRuleDto
                {
                    StartTime = new TimeSpan(6,0,0),
                    EndTime = new TimeSpan(7,0,0),
                    Amount = 10
                }
            }
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Validate_WithInvalidCode_ShouldFail()
    {
        var command = new AddCityCommand
        {
            Name = "City",
            Code = string.Empty,
            MaxDailyTax = 60
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}

