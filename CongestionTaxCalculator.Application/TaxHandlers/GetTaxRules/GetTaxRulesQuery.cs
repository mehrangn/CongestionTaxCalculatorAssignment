using MediatR;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Application.TaxHandlers.GetTaxRules;

public class GetTaxRulesQuery : IRequest<GetTaxRulesResponse>
{
    public string CityCode { get; set; } = "GOT";
}

