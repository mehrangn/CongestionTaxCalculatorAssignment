namespace CongestionTaxCalculator.Application.Common.City.Responses;

public class CalculateTaxResponse
{
    public decimal TaxAmount { get; set; }
    public string Currency { get; set; } = "SEK";
}

