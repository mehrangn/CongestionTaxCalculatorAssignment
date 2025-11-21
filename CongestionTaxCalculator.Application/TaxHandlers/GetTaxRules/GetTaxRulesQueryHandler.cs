using MediatR;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.Common.City.Responses;
using CongestionTaxCalculator.Application.Common.City.Dtos;

namespace CongestionTaxCalculator.Application.TaxHandlers.GetTaxRules;

public class GetTaxRulesQueryHandler : IRequestHandler<GetTaxRulesQuery, GetTaxRulesResponse>
{
    private readonly ICityRepository _cityRepository;

    public GetTaxRulesQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<GetTaxRulesResponse> Handle(GetTaxRulesQuery request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByCodeAsync(request.CityCode, cancellationToken);
        if (city == null)
        {
            throw new ArgumentException($"City with code '{request.CityCode}' not found");
        }

        var taxRules = await _cityRepository.GetTaxRulesByCityIdAsync(city.Id, cancellationToken);
        return BuildTaxRuleResponse(city, taxRules);
    }

    private static GetTaxRulesResponse BuildTaxRuleResponse(City city, ICollection<TaxRule> taxRules)
    {
        return new GetTaxRulesResponse
        {
            CityName = city.Name,
            MaxDailyTax = city.MaxDailyTax,
            SingleChargeMinutes = city.SingleChargeMinutes,
            TaxRules = taxRules.Select(tr => new TaxRuleDto
            {
                StartTime = tr.StartTime,
                EndTime = tr.EndTime,
                Amount = tr.Amount
            }).ToList()
        };
    }
}

