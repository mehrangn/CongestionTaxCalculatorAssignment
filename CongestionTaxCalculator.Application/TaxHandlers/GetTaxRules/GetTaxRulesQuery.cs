using MediatR;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.Common.City.Responses;

namespace CongestionTaxCalculator.Application.TaxHandlers.GetTaxRules;

public class GetTaxRulesQuery : IRequest<GetTaxRulesResponse>
{
    public string CityCode { get; set; } = "GOT";
}

