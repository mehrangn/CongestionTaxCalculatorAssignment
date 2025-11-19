using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CongestionTaxCalculator.Application.CityHandlers.DeleteCity;

namespace CongestionTaxCalculator.Tests.Application.Validators;

[TestClass]
public class DeleteCityCommandValidatorTests
{
    private readonly DeleteCityCommandValidator _validator;

    public DeleteCityCommandValidatorTests()
    {
        _validator = new DeleteCityCommandValidator();
    }

    [TestMethod]
    public void Validate_WithValidCode_ShouldPass()
    {
        var command = new DeleteCityCommand { Code = "AAA" };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Validate_WithEmptyCode_ShouldFail()
    {
        var command = new DeleteCityCommand { Code = string.Empty };
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }
}

