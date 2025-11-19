using FluentValidation;

namespace CongestionTaxCalculator.Application.TaxHandlers.CalculateTax;

public class CalculateTaxCommandValidator : AbstractValidator<CalculateTaxCommand>
{
    public CalculateTaxCommandValidator()
    {
        RuleFor(x => x.VehicleType)
            .IsInEnum()
            .WithMessage("Vehicle type is required.");

        RuleFor(x => x.Dates)
            .NotNull()
            .WithMessage("Dates cannot be null.")
            .NotEmpty()
            .WithMessage("At least one date is required.");

        RuleFor(x => x.CityCode)
            .NotEmpty()
            .WithMessage("City code is required.")
            .MaximumLength(10)
            .WithMessage("City code must not exceed 10 characters.");

        RuleForEach(x => x.Dates)
            .Must(date => date.Year == 2013)
            .WithMessage("Only dates from year 2013 are supported.");
    }
}

