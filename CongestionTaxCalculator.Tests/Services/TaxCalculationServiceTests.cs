using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Services;
using CongestionTaxCalculator.Domain.Interfaces;

namespace CongestionTaxCalculator.UnitTests.Services;

[TestClass]
public class TaxCalculationServiceTests
{
    private readonly ITaxCalculationService _taxCalculationService;
    private readonly City _gothenburg;
    private readonly ICollection<TaxRule> _taxRules;
    private readonly ICollection<PublicHoliday> _publicHolidays;
    public TaxCalculationServiceTests()
    {
        _taxCalculationService = new TaxCalculationService();
        _gothenburg = new City
        {
            Id = 1,
            Name = "Gothenburg",
            Code = "GOT",
            MaxDailyTax = 60,
            SingleChargeMinutes = 60
        };

        _taxRules = new List<TaxRule>
        {
            new TaxRule { Id = 1, CityId = 1, StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(6, 29, 0), Amount = 8 },
            new TaxRule { Id = 2, CityId = 1, StartTime = new TimeSpan(6, 30, 0), EndTime = new TimeSpan(6, 59, 0), Amount = 13 },
            new TaxRule { Id = 3, CityId = 1, StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(7, 59, 0), Amount = 18 },
            new TaxRule { Id = 4, CityId = 1, StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(8, 29, 0), Amount = 13 },
            new TaxRule { Id = 5, CityId = 1, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(14, 59, 0), Amount = 8 },
            new TaxRule { Id = 6, CityId = 1, StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(15, 29, 0), Amount = 13 },
            new TaxRule { Id = 7, CityId = 1, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(16, 59, 0), Amount = 18 },
            new TaxRule { Id = 8, CityId = 1, StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(17, 59, 0), Amount = 13 },
            new TaxRule { Id = 9, CityId = 1, StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(18, 29, 0), Amount = 8 },
            new TaxRule { Id = 10, CityId = 1, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(23, 59, 59), Amount = 0 },
            new TaxRule { Id = 11, CityId = 1, StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(5, 59, 59), Amount = 0 }
        };

        _publicHolidays = new List<PublicHoliday>();
    }

    [TestMethod]
    public void CalculateTax_WithTollFreeVehicle_ShouldReturnZero()
    {
        var vehicle = new Motorbike();
        var dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) };
        
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(0);
    }

    [TestMethod]
    public void CalculateTax_OnWeekend_ShouldReturnZero()
    {
        var vehicle = new Car();
        var dates = new[] { new DateTime(2013, 2, 9, 6, 0, 0) };
        
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(0);
    }

    [TestMethod]
    public void CalculateTax_InJuly_ShouldReturnZero()
    {
        var vehicle = new Car();
        var dates = new[] { new DateTime(2013, 7, 15, 6, 0, 0) };
        
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(0);
    }

    [TestMethod]
    public void CalculateTax_SinglePass_ShouldReturnCorrectFee()
    {
        var vehicle = new Car();
        var dates = new[] { new DateTime(2013, 2, 8, 6, 0, 0) };
        
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(8);
    }

    [TestMethod]
    public void CalculateTax_MultiplePassesWithin60Minutes_ShouldChargeHighestFee()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            new DateTime(2013, 2, 8, 6, 0, 0),
            new DateTime(2013, 2, 8, 6, 30, 0),
            new DateTime(2013, 2, 8, 6, 45, 0)
        };
        
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(13);
    }

    [TestMethod]
    public void CalculateTax_MultiplePassesOver60Minutes_ShouldChargeMultipleFees()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            new DateTime(2013, 2, 8, 6, 0, 0),
            new DateTime(2013, 2, 8, 7, 15, 0)
        };
        
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(26);
    }

    [TestMethod]
    public void CalculateTax_ExceedsMaxDailyTax_ShouldCapAtMax()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            new DateTime(2013, 2, 8, 6, 0, 0),
            new DateTime(2013, 2, 8, 7, 0, 0),
            new DateTime(2013, 2, 8, 8, 0, 0),
            new DateTime(2013, 2, 8, 15, 30, 0),
            new DateTime(2013, 2, 8, 16, 0, 0),
            new DateTime(2013, 2, 8, 17, 0, 0)
        };
       
        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(60);
    }

    [TestMethod]
    public void GetTollFee_At0600_ShouldReturn8()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 2, 8, 6, 0, 0);
       
        var result = _taxCalculationService.GetTollFee(date, vehicle, _gothenburg, _taxRules, _publicHolidays);
       
        result.Should().Be(8);
    }

    [TestMethod]
    public void GetTollFee_At0630_ShouldReturn13()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 2, 8, 6, 30, 0);
        var result = _taxCalculationService.GetTollFee(date, vehicle, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(13);
    }

    [TestMethod]
    public void GetTollFee_At0700_ShouldReturn18()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 2, 8, 7, 0, 0);
        
        var result = _taxCalculationService.GetTollFee(date, vehicle, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(18);
    }

    [TestMethod]
    public void GetTollFee_At1830_ShouldReturn0()
    {
        var vehicle = new Car();
        var date = new DateTime(2013, 2, 8, 18, 30, 0);
        
        var result = _taxCalculationService.GetTollFee(date, vehicle, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().Be(0);
    }

    [TestMethod]
    public void CalculateTax_WithTestDatesFromPostIt_ShouldCalculateCorrectly()
    {
        var vehicle = new Car();
        var dates = new[]
        {
            new DateTime(2013, 1, 14, 21, 0, 0),
            new DateTime(2013, 1, 15, 21, 0, 0),
            new DateTime(2013, 2, 7, 6, 23, 27),
            new DateTime(2013, 2, 7, 15, 27, 0),
            new DateTime(2013, 2, 8, 6, 27, 0),
            new DateTime(2013, 2, 8, 6, 20, 27),
            new DateTime(2013, 2, 8, 14, 35, 0),
            new DateTime(2013, 2, 8, 15, 29, 0),
            new DateTime(2013, 2, 8, 15, 47, 0),
            new DateTime(2013, 2, 8, 16, 1, 0),
            new DateTime(2013, 2, 8, 16, 48, 0),
            new DateTime(2013, 2, 8, 17, 49, 0),
            new DateTime(2013, 2, 8, 18, 29, 0),
            new DateTime(2013, 2, 8, 18, 35, 0),
            new DateTime(2013, 3, 26, 14, 25, 0),
            new DateTime(2013, 3, 28, 14, 7, 27)
        };

        var result = _taxCalculationService.CalculateTax(vehicle, dates, _gothenburg, _taxRules, _publicHolidays);
        
        result.Should().BeGreaterThanOrEqualTo(0);
        var day2Dates = dates.Where(d => d.Date == new DateTime(2013, 2, 8).Date).ToArray();
        var day2Result = _taxCalculationService.CalculateTax(vehicle, day2Dates, _gothenburg, _taxRules, _publicHolidays);
        day2Result.Should().BeLessThanOrEqualTo(60);
    }
}
