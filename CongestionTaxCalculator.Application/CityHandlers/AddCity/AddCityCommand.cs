using MediatR;
using CongestionTaxCalculator.Application.Common.City.Dtos;
using CongestionTaxCalculator.Application.Common.City.Responses;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Application.CityHandlers.AddCity;

public class AddCityCommand : IRequest<CityResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public decimal MaxDailyTax { get; set; }
    public int SingleChargeMinutes { get; set; } = 60;
    public bool IsActive { get; set; } = true;
    public IList<TaxRuleDto> TaxRules { get; set; } = new List<TaxRuleDto>();
}

