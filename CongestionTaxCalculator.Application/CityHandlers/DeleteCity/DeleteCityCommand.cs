using MediatR;

namespace CongestionTaxCalculator.Application.CityHandlers.DeleteCity;

public class DeleteCityCommand : IRequest<bool>
{
    public string Code { get; set; } = string.Empty;
}

