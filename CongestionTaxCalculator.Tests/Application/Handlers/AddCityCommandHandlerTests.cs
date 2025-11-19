using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using CongestionTaxCalculator.Application.Common.City.Dtos;
using CongestionTaxCalculator.Application.CityHandlers.AddCity;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Tests.Application.Handlers;

[TestClass]
public class AddCityCommandHandlerTests
{
    private readonly ICityRepository _cityRepository;
    private readonly AddCityCommandHandler _handler;

    public AddCityCommandHandlerTests()
    {
        _cityRepository = Substitute.For<ICityRepository>();
        _handler = new AddCityCommandHandler(_cityRepository);
    }

    [TestMethod]
    public async Task Handle_WithValidCommand_ShouldCreateCity()
    {
        var command = new AddCityCommand
        {
            Name = "Test City",
            Code = "TST",
            MaxDailyTax = 50,
            SingleChargeMinutes = 60,
            TaxRules = new List<TaxRuleDto>
            {
                new TaxRuleDto
                {
                    StartTime = new TimeSpan(6,0,0),
                    EndTime = new TimeSpan(6,30,0),
                    Amount = 10
                }
            }
        };

        _cityRepository.CityCodeExistsAsync("TST", Arg.Any<CancellationToken>())
            .Returns(false);

        _cityRepository.AddCityAsync(Arg.Any<City>(), Arg.Any<IEnumerable<TaxRule>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new City
            {
                Id = 42,
                Name = command.Name,
                Code = command.Code,
                MaxDailyTax = command.MaxDailyTax,
                SingleChargeMinutes = command.SingleChargeMinutes,
                IsActive = command.IsActive
            }));

        var response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response.Id.Should().Be(42);
        response.Code.Should().Be("TST");
    }

    [TestMethod]
    public async Task Handle_WithDuplicateCode_ShouldThrowArgumentException()
    {
        var command = new AddCityCommand
        {
            Name = "Duplicate City",
            Code = "DUP",
            MaxDailyTax = 60
        };

        _cityRepository.CityCodeExistsAsync("DUP", Arg.Any<CancellationToken>())
            .Returns(true);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}

