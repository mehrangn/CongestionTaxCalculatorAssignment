using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CongestionTaxCalculator.Application.CityHandlers.AddCity;
using CongestionTaxCalculator.Application.CityHandlers.DeleteCity;
using CongestionTaxCalculator.Application.Common.City.Requests;
using CongestionTaxCalculator.Application.Common.City.Responses;
using CongestionTaxCalculator.Application.Common.City.Dtos;

namespace CongestionTaxCalculator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CityResponse>> AddCity([FromBody] AddCityRequest request, CancellationToken cancellationToken)
    {
        var command = new AddCityCommand
        {
            Name = request.Name,
            Code = request.Code,
            MaxDailyTax = request.MaxDailyTax,
            SingleChargeMinutes = request.SingleChargeMinutes,
            IsActive = request.IsActive,
            TaxRules = request.TaxRules.Select(tr => new TaxRuleDto
            {
                StartTime = tr.StartTime,
                EndTime = tr.EndTime,
                Amount = tr.Amount
            }).ToList()
        };

        try
        {
            var response = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(AddCity), new { code = response.Code }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{code}")]
    public async Task<ActionResult> DeleteCity(string code, CancellationToken cancellationToken)
    {
        var command = new DeleteCityCommand { Code = code };
        try
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

