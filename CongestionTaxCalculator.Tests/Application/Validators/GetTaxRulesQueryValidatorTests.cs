using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CongestionTaxCalculator.Application.TaxHandlers.GetTaxRules;
namespace CongestionTaxCalculator.UnitTests.Application.Validators;

[TestClass]
public class GetTaxRulesQueryValidatorTests
{
    private readonly GetTaxRulesQueryValidator _validator;

    public GetTaxRulesQueryValidatorTests()
    {
        _validator = new GetTaxRulesQueryValidator();
    }

    [TestMethod]
    public void Validate_WithValidQuery_ShouldPass()
    {
        var query = new GetTaxRulesQuery
        {
            CityCode = "GOT"
        };
        
        var result = _validator.Validate(query);
        
        result.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void Validate_WithEmptyCityCode_ShouldFail()
    {
        var query = new GetTaxRulesQuery
        {
            CityCode = string.Empty
        };
        
        var result = _validator.Validate(query);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CityCode");
    }

    [TestMethod]
    public void Validate_WithCityCodeTooLong_ShouldFail()
    {
        var query = new GetTaxRulesQuery
        {
            CityCode = new string('A', 11)
        };
        
        var result = _validator.Validate(query);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CityCode");
    }
}
