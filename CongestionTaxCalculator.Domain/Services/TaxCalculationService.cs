using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Core;
using CongestionTaxCalculator.Domain.Interfaces;

namespace CongestionTaxCalculator.Domain.Services;

public class TaxCalculationService : ITaxCalculationService
{
    public decimal CalculateTax(
        Vehicle vehicle, 
        DateTime[] dates, 
        City city, 
        ICollection<TaxRule> taxRules, 
        ICollection<PublicHoliday> publicHolidays)
    {
        if (dates == null || dates.Length == 0)
        {
            return 0;
        }

        if (IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        var sortedDates = dates.OrderBy(d => d).ToArray();
        DateTime intervalStart = sortedDates[0];
        decimal totalFee = 0;
        decimal maxFeeInInterval = 0;

        foreach (DateTime date in sortedDates)
        {
            decimal nextFee = GetTollFee(date, vehicle, city, taxRules, publicHolidays);

            var timeDiff = date - intervalStart;
            double minutes = timeDiff.TotalMinutes;

            if (minutes <= city.SingleChargeMinutes)
            {
                maxFeeInInterval = Math.Max(maxFeeInInterval, nextFee);
            }
            else
            {
                totalFee += maxFeeInInterval;
                intervalStart = date;
                maxFeeInInterval = nextFee;
            }
        }

        totalFee += maxFeeInInterval;
        if (totalFee > city.MaxDailyTax)
        {
            totalFee = city.MaxDailyTax;
        }

        return totalFee;
    }

    public decimal GetTollFee(
        DateTime date, 
        Vehicle vehicle, 
        City city, 
        ICollection<TaxRule> taxRules, 
        ICollection<PublicHoliday> publicHolidays)
    {
        if (IsTollFreeDate(date, city, publicHolidays) || IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        var timeOfDay = date.TimeOfDay;
        var applicableRule = taxRules
            .Where(rule => IsTimeInRange(timeOfDay, rule.StartTime, rule.EndTime))
            .OrderByDescending(rule => rule.Amount)
            .FirstOrDefault();

        return applicableRule?.Amount ?? 0;
    }

    public bool IsTollFreeDate(DateTime date, City city, ICollection<PublicHoliday> publicHolidays)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        if (date.Month == 7)
        {
            return true;
        }
        var dateOnly = date.Date;
        var isHoliday = publicHolidays.Any(h => h.Date.Date == dateOnly);
        var isDayBeforeHoliday = publicHolidays.Any(h => h.IsDayBeforeHoliday && h.Date.Date.AddDays(1) == dateOnly);

        return isHoliday || isDayBeforeHoliday;
    }

    private bool IsTimeInRange(TimeSpan time, TimeSpan start, TimeSpan end)
    {
        if (end < start)
        {
            return time >= start || time <= end;
        }

        return time >= start && time <= end;
    }

    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            return false;
        }

        return vehicle.IsTollFree;
    }
}

