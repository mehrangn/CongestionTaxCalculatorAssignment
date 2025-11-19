using MediatR;
using CongestionTaxCalculator.Application.Interfaces;

namespace CongestionTaxCalculator.Application.CityHandlers.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, bool>
{
    private readonly ICityRepository _cityRepository;

    public DeleteCityCommandHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _cityRepository.DeleteCityByCodeAsync(request.Code, cancellationToken);
        if (!deleted)
        {
            throw new ArgumentException($"City with code '{request.Code}' not found");
        }

        return true;
    }
}

