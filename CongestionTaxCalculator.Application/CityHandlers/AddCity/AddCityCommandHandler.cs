using MediatR;
using CongestionTaxCalculator.Application.Common.City.Responses;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Application.CityHandlers.AddCity;

public class AddCityCommandHandler : IRequestHandler<AddCityCommand, CityResponse>
{
    private readonly ICityRepository _cityRepository;

    public AddCityCommandHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<CityResponse> Handle(AddCityCommand request, CancellationToken cancellationToken)
    {
        var codeExists = await _cityRepository.CityCodeExistsAsync(request.Code, cancellationToken);
        if (codeExists)
        {
            throw new ArgumentException($"City with code '{request.Code}' already exists");
        }
        City city = BuildCity(request);
        List<TaxRule> taxRules = BuildTaxRules(request);

        var createdCity = await _cityRepository.AddCityAsync(city, taxRules, cancellationToken);
        var cityResponse = BuildCityResponse(createdCity);

        return cityResponse;
    }

    private static City BuildCity(AddCityCommand request)
    {
        return new City
        {
            Name = request.Name,
            Code = request.Code,
            MaxDailyTax = request.MaxDailyTax,
            SingleChargeMinutes = request.SingleChargeMinutes,
            IsActive = request.IsActive
        };
    }

    private static List<TaxRule> BuildTaxRules(AddCityCommand request)
    {
        return request.TaxRules.Select(tr => new TaxRule
        {
            StartTime = tr.StartTime,
            EndTime = tr.EndTime,
            Amount = tr.Amount
        }).ToList();
    }

    private CityResponse BuildCityResponse(City createdCity)
    {
        return new CityResponse
        {
            Id = createdCity.Id,
            Name = createdCity.Name,
            Code = createdCity.Code,
            MaxDailyTax = createdCity.MaxDailyTax,
            SingleChargeMinutes = createdCity.SingleChargeMinutes,
            IsActive = createdCity.IsActive
        };
    }
}

