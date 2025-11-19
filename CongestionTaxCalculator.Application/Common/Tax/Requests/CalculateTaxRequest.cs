namespace CongestionTaxCalculator.Application.Common.Tax.Requests;

public class CalculateTaxRequest
{
    public string VehicleType { get; set; } = string.Empty;
    public DateTime[] Dates { get; set; } = Array.Empty<DateTime>();
    public string? CityCode { get; set; }
}

