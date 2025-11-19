using FluentValidation;

namespace CongestionTaxCalculator.Application.CityHandlers.DeleteCity;

public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(10);
    }
}

