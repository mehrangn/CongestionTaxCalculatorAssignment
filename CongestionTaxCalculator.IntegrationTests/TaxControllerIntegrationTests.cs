using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CongestionTaxCalculator.Infrastructure.Data;

namespace CongestionTaxCalculator.IntegrationTests;

[TestClass]
public class TaxControllerIntegrationTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TaxControllerIntegrationTests()
    {
        var databaseName = $"TestDb_{Guid.NewGuid()}";
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                });
            });
        });
        _client = _factory.CreateClient();
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData.Seed(context);
        }
    }

    [TestMethod]
    public async Task CalculateTax_WithValidCarAndDates_ShouldReturnTaxAmount()
    {
        var request = new
        {
            VehicleType = "Car",
            Dates = new[]
            {
                new DateTime(2013, 2, 8, 6, 0, 0),
                new DateTime(2013, 2, 8, 7, 0, 0)
            },
            CityCode = "GOT"
        };
        
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        
        result.Should().NotBeNull();
        result!.TaxAmount.Should().BeGreaterThan(0);
        result.Currency.Should().Be("SEK");
    }
    [TestMethod]
    public async Task CalculateTax_WithTollFreeVehicle_ShouldReturnZero()
    {
        var request = new
        {
            VehicleType = "Motorbike",
            Dates = new[]
            {
                new DateTime(2013, 2, 8, 6, 0, 0)
            },
            CityCode = "GOT"
        };
       
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        
        result.Should().NotBeNull();
        result!.TaxAmount.Should().Be(0);
    }
    [TestMethod]
    public async Task CalculateTax_OnWeekend_ShouldReturnZero()
    {
        var request = new
        {
            VehicleType = "Car",
            Dates = new[]
            {
                new DateTime(2013, 2, 9, 6, 0, 0)
            },
            CityCode = "GOT"
        };
        
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request);
       
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        
        result.Should().NotBeNull();
        result!.TaxAmount.Should().Be(0);
    }
    [TestMethod]
    public async Task CalculateTax_WithInvalidVehicleType_ShouldReturnBadRequest()
    {
        var request = new
        {
            VehicleType = "InvalidType",
            Dates = new[]
            {
                new DateTime(2013, 2, 8, 6, 0, 0)
            },
            CityCode = "GOT"
        };
        
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [TestMethod]
    public async Task CalculateTax_WithEmptyDates_ShouldReturnBadRequest()
    {
        var request = new
        {
            VehicleType = "Car",
            Dates = Array.Empty<DateTime>(),
            CityCode = "GOT"
        };
        
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [TestMethod]
    public async Task CalculateTax_WithPostItDates_ShouldCalculateCorrectly()
    {
        var request = new
        {
            VehicleType = "Car",
            Dates = new[]
            {
                new DateTime(2013, 2, 8, 6, 27, 0),
                new DateTime(2013, 2, 8, 6, 20, 27),
                new DateTime(2013, 2, 8, 14, 35, 0),
                new DateTime(2013, 2, 8, 15, 29, 0),
                new DateTime(2013, 2, 8, 15, 47, 0),
                new DateTime(2013, 2, 8, 16, 1, 0),
                new DateTime(2013, 2, 8, 16, 48, 0),
                new DateTime(2013, 2, 8, 17, 49, 0),
                new DateTime(2013, 2, 8, 18, 29, 0),
                new DateTime(2013, 2, 8, 18, 35, 0)
            },
            CityCode = "GOT"
        };
        
        var response = await _client.PostAsJsonAsync("/api/tax/calculate", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<CalculateTaxResponse>();
        
        result.Should().NotBeNull();
        result!.TaxAmount.Should().BeGreaterThan(0);
        result.TaxAmount.Should().BeLessThanOrEqualTo(60);
    }
    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}
public class CalculateTaxResponse
{
    public decimal TaxAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
}
