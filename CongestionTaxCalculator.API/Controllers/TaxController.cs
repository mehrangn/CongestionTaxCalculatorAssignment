using Microsoft.AspNetCore.Mvc;
using MediatR;
using CongestionTaxCalculator.Application.TaxHandlers.CalculateTax;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.Common.Tax.Requests;
using CongestionTaxCalculator.Application.Common.City.Responses;

namespace CongestionTaxCalculator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaxController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<CalculateTaxResponse>> CalculateTax([FromBody] CalculateTaxRequest request)
    {
        if (request == null || request.Dates == null || request.Dates.Length == 0)
        {
            return BadRequest("Vehicle type and dates are required");
        }

        if (!Enum.TryParse<VehicleType>(request.VehicleType, true, out var vehicleType))
        {
            return BadRequest($"Unknown vehicle type: {request.VehicleType}");
        }

        var command = new CalculateTaxCommand
        {
            VehicleType = vehicleType,
            Dates = request.Dates,
            CityCode = request.CityCode ?? "GOT"
        };

        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
