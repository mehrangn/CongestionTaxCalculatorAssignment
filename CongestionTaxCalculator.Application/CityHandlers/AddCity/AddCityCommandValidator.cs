using FluentValidation;
using CongestionTaxCalculator.Application.Common.City.Dtos;

namespace CongestionTaxCalculator.Application.CityHandlers.AddCity;

public class AddCityCommandValidator : AbstractValidator<AddCityCommand>
{
    public AddCityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.MaxDailyTax)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SingleChargeMinutes)
            .GreaterThan(0)
            .LessThanOrEqualTo(180);

        RuleForEach(x => x.TaxRules).SetValidator(new TaxRuleDtoValidator());
    }

    private class TaxRuleDtoValidator : AbstractValidator<TaxRuleDto>
    {
        public TaxRuleDtoValidator()
        {
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        }
    }

}

