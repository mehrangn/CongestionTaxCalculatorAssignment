using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.TaxHandlers.GetTaxRules;
namespace CongestionTaxCalculator.UnitTests.Application.Handlers;

[TestClass]
public class GetTaxRulesQueryHandlerTests
{
    private readonly ICityRepository _cityRepository;
    private readonly GetTaxRulesQueryHandler _handler;

    public GetTaxRulesQueryHandlerTests()
    {
        _cityRepository = Substitute.For<ICityRepository>();
        _handler = new GetTaxRulesQueryHandler(_cityRepository);
    }

    [TestMethod]
    public async Task Handle_WithValidQuery_ShouldReturnTaxRules()
    {
        var city = new City
        {
            Id = 1,
            Name = "Gothenburg",
            Code = "GOT",
            MaxDailyTax = 60,
            SingleChargeMinutes = 60
        };
        var taxRules = new List<TaxRule>
        {
            new TaxRule { Id = 1, CityId = 1, StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(6, 29, 0), Amount = 8 },
            new TaxRule { Id = 2, CityId = 1, StartTime = new TimeSpan(6, 30, 0), EndTime = new TimeSpan(6, 59, 0), Amount = 13 }
        };
        _cityRepository.GetByCodeAsync("GOT", Arg.Any<CancellationToken>())
            .Returns(city);
        _cityRepository.GetTaxRulesByCityIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(taxRules);
        var query = new GetTaxRulesQuery
        {
            CityCode = "GOT"
        };
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.CityName.Should().Be("Gothenburg");
        result.MaxDailyTax.Should().Be(60);
        result.SingleChargeMinutes.Should().Be(60);
        result.TaxRules.Should().HaveCount(2);
        result.TaxRules.First().Amount.Should().Be(8);
    }

    [TestMethod]
    public async Task Handle_WithInvalidCityCode_ShouldThrowArgumentException()
    {
        _cityRepository.GetByCodeAsync("INVALID", Arg.Any<CancellationToken>())
            .Returns((City?)null);
        var query = new GetTaxRulesQuery
        {
            CityCode = "INVALID"
        };
       
        var act = async () => await _handler.Handle(query, CancellationToken.None);
       
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
