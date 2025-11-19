using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Infrastructure.Data;

public static class SeedData
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Cities.Any())
        {
            var gothenburg = new City
            {
                Name = "Gothenburg",
                Code = "GOT",
                MaxDailyTax = 60,
                SingleChargeMinutes = 60,
                IsActive = true
            };

            context.Cities.Add(gothenburg);
            context.SaveChanges();

            var taxRules = new List<TaxRule>
            {
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(6, 29, 0), Amount = 8 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(6, 30, 0), EndTime = new TimeSpan(6, 59, 0), Amount = 13 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(7, 59, 0), Amount = 18 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(8, 29, 0), Amount = 13 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(14, 59, 0), Amount = 8 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(15, 29, 0), Amount = 13 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(16, 59, 0), Amount = 18 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(17, 59, 0), Amount = 13 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(18, 29, 0), Amount = 8 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(23, 59, 59), Amount = 0 },
                new TaxRule { CityId = gothenburg.Id, StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(5, 59, 59), Amount = 0 }
            };

            context.TaxRules.AddRange(taxRules);
            context.SaveChanges();
        }

        if (!context.PublicHolidays.Any())
        {
            var publicHolidays = new List<PublicHoliday>
            {
                new PublicHoliday { Date = new DateTime(2013, 1, 1), Name = "New Year's Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 3, 28), Name = "Maundy Thursday", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 3, 29), Name = "Good Friday", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 4, 1), Name = "Easter Monday", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 4, 30), Name = "Walpurgis Night", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 5, 1), Name = "Labour Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 5, 8), Name = "Ascension Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 5, 9), Name = "Ascension Day Eve", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 6, 5), Name = "Whit Monday", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 6, 6), Name = "National Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 6, 21), Name = "Midsummer Eve", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 11, 1), Name = "All Saints' Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 12, 24), Name = "Christmas Eve", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 12, 25), Name = "Christmas Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 12, 26), Name = "Boxing Day", IsDayBeforeHoliday = false },
                new PublicHoliday { Date = new DateTime(2013, 12, 31), Name = "New Year's Eve", IsDayBeforeHoliday = false }
            };

            foreach (var holiday in publicHolidays.ToList())
            {
                if (holiday.Date.DayOfWeek != DayOfWeek.Monday &&
                    holiday.Date.DayOfWeek != DayOfWeek.Saturday &&
                    holiday.Date.DayOfWeek != DayOfWeek.Sunday)
                {
                    publicHolidays.Add(new PublicHoliday
                    {
                        Date = holiday.Date.AddDays(-1),
                        Name = $"Day before {holiday.Name}",
                        IsDayBeforeHoliday = true
                    });
                }
            }

            context.PublicHolidays.AddRange(publicHolidays);
            context.SaveChanges();
        }
    }
}

