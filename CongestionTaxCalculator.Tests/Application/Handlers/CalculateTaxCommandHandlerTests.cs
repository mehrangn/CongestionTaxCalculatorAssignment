using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces;
using CongestionTaxCalculator.Application.TaxHandlers.CalculateTax;
using CongestionTaxCalculator.Domain.Entities.Core;
namespace CongestionTaxCalculator.UnitTests.Application.Handlers;

[TestClass]
public class CalculateTaxCommandHandlerTests
{
    private readonly ITaxCalculationService _taxCalculationService;
    private readonly ICityRepository _cityRepository;
    private readonly IVehicleFactory _vehicleFactory;
    private readonly CalculateTaxCommandHandler _handler;

    public CalculateTaxCommandHandlerTests()
    {
        _taxCalculationService = Substitute.For<ITaxCalculationService>();
        _cityRepository = Substitute.For<ICityRepository>();
        _vehicleFactory = Substitute.For<IVehicleFactory>();
        _handler = new CalculateTaxCommandHandler(
            _taxCalculationService,
            _cityRepository,
            _vehicleFactory);
    }

    [TestMethod]
    public async Task Handle_WithValidRequest_ShouldReturnTaxAmount()
    {
        var city = new City { Id = 1, Code = "GOT", MaxDailyTax = 60, SingleChargeMinutes = 60 };
        var vehicle = new Car();
        var taxRules = new List<TaxRule>();
        var publicHolidays = new List<PublicHoliday>();
        var dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) };
        _cityRepository.GetByCodeAsync("GOT", Arg.Any<CancellationToken>())
            .Returns(city);
        _cityRepository.GetTaxRulesByCityIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(taxRules);
        _cityRepository.GetPublicHolidaysAsync(Arg.Any<CancellationToken>())
            .Returns(publicHolidays);
        _cityRepository.GetPublicHolidaysAsync(Arg.Any<CancellationToken>())
            .Returns(publicHolidays);
        _vehicleFactory.CreateVehicle(VehicleType.Car)
            .Returns(vehicle);
        _taxCalculationService.CalculateTax(vehicle, dates, city, taxRules, publicHolidays)
            .Returns(8);
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = dates,
            CityCode = "GOT"
        };
       
        var result = await _handler.Handle(command, CancellationToken.None);
       
        result.Should().NotBeNull();
        result.TaxAmount.Should().Be(8);
        result.Currency.Should().Be("SEK");
    }

    [TestMethod]
    public async Task Handle_WithInvalidCityCode_ShouldThrowArgumentException()
    {
        _cityRepository.GetByCodeAsync("INVALID", Arg.Any<CancellationToken>())
            .Returns((City?)null);
        var command = new CalculateTaxCommand
        {
            VehicleType = VehicleType.Car,
            Dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) },
            CityCode = "INVALID"
        };
        
        var act = async () => await _handler.Handle(command, CancellationToken.None);
       
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [TestMethod]
    public async Task Handle_WithInvalidVehicleType_ShouldThrowArgumentException()
    {
        var city = new City { Id = 1, Code = "GOT" };
        _cityRepository.GetByCodeAsync("GOT", Arg.Any<CancellationToken>())
            .Returns(city);
        _vehicleFactory.CreateVehicle((VehicleType)999)
            .Returns((Vehicle?)null);
        var command = new CalculateTaxCommand
        {
            VehicleType = (VehicleType)999,
            Dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) },
            CityCode = "GOT"
        };
       
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
