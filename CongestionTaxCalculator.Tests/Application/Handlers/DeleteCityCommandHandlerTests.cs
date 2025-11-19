using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using CongestionTaxCalculator.Application.CityHandlers.DeleteCity;
using CongestionTaxCalculator.Application.Interfaces;

namespace CongestionTaxCalculator.Tests.Application.Handlers;

[TestClass]
public class DeleteCityCommandHandlerTests
{
    private readonly ICityRepository _cityRepository;
    private readonly DeleteCityCommandHandler _handler;

    public DeleteCityCommandHandlerTests()
    {
        _cityRepository = Substitute.For<ICityRepository>();
        _handler = new DeleteCityCommandHandler(_cityRepository);
    }

    [TestMethod]
    public async Task Handle_WithExistingCity_ShouldReturnTrue()
    {
        _cityRepository.DeleteCityByCodeAsync("TST", Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new DeleteCityCommand { Code = "TST" };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task Handle_WithMissingCity_ShouldThrowArgumentException()
    {
        _cityRepository.DeleteCityByCodeAsync("MISSING", Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new DeleteCityCommand { Code = "MISSING" };

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}

