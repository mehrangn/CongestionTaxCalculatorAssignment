using FluentValidation;

namespace CongestionTaxCalculator.Application.TaxHandlers.GetTaxRules;

public class GetTaxRulesQueryValidator : AbstractValidator<GetTaxRulesQuery>
{
    public GetTaxRulesQueryValidator()
    {
        RuleFor(x => x.CityCode)
            .NotEmpty()
            .WithMessage("City code is required.")
            .MaximumLength(10)
            .WithMessage("City code must not exceed 10 characters.");
    }
}

