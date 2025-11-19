using MediatR;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Application.Common.City.Responses;

namespace CongestionTaxCalculator.Application.TaxHandlers.CalculateTax;

public class CalculateTaxCommandHandler : IRequestHandler<CalculateTaxCommand, CalculateTaxResponse>
{
    private readonly ITaxCalculationService _taxCalculationService;
    private readonly ICityRepository _cityRepository;
    private readonly IVehicleFactory _vehicleFactory;

    public CalculateTaxCommandHandler(
        ITaxCalculationService taxCalculationService,
        ICityRepository cityRepository,
        IVehicleFactory vehicleFactory)
    {
        _taxCalculationService = taxCalculationService;
        _cityRepository = cityRepository;
        _vehicleFactory = vehicleFactory;
    }

    public async Task<CalculateTaxResponse> Handle(CalculateTaxCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByCodeAsync(request.CityCode, cancellationToken);
        if (city == null)
        {
            throw new ArgumentException($"City with code '{request.CityCode}' not found");
        }

        var vehicle = _vehicleFactory.CreateVehicle(request.VehicleType);
        if (vehicle == null)
        {
            throw new ArgumentException($"Unknown vehicle type: {request.VehicleType}");
        }

        var taxRules = await _cityRepository.GetTaxRulesByCityIdAsync(city.Id, cancellationToken);
        var publicHolidays = await _cityRepository.GetPublicHolidaysAsync(cancellationToken);
        decimal taxAmount = CallCalculateTax(request, city, vehicle, taxRules, publicHolidays);

        return new CalculateTaxResponse
        {
            TaxAmount = taxAmount,
            Currency = "SEK"
        };
    }

    private decimal CallCalculateTax(CalculateTaxCommand request, City city, Domain.Entities.Core.Vehicle vehicle, ICollection<TaxRule> taxRules, ICollection<PublicHoliday> publicHolidays)
    {
        return _taxCalculationService.CalculateTax(
            vehicle,
            request.Dates,
            city,
            taxRules,
            publicHolidays);
    }
}

