using MediatR;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.Common.City.Responses;

namespace CongestionTaxCalculator.Application.TaxHandlers.CalculateTax;

public class CalculateTaxCommand : IRequest<CalculateTaxResponse>
{
    public VehicleType VehicleType { get; set; }
    public DateTime[] Dates { get; set; } = Array.Empty<DateTime>();
    public string CityCode { get; set; } = "GOT"; // Default to Gothenburg
}

