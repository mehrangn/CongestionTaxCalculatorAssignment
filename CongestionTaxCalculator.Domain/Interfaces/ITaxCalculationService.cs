using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Core;

namespace CongestionTaxCalculator.Domain.Interfaces;

public interface ITaxCalculationService
{
    decimal CalculateTax(Vehicle vehicle, DateTime[] dates, City city, ICollection<TaxRule> taxRules, ICollection<PublicHoliday> publicHolidays);
    decimal GetTollFee(DateTime date, Vehicle vehicle, City city, ICollection<TaxRule> taxRules, ICollection<PublicHoliday> publicHolidays);
    bool IsTollFreeDate(DateTime date, City city, ICollection<PublicHoliday> publicHolidays);
    bool IsTollFreeVehicle(Vehicle vehicle);
}

