using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CongestionTaxCalculator.Infrastructure.Data;
using CongestionTaxCalculator.Application.Common.City.Responses;

namespace CongestionTaxCalculator.IntegrationTests;

[TestClass]
public class CitiesControllerIntegrationTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CitiesControllerIntegrationTests()
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

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        SeedData.Seed(context);
    }

    [TestMethod]
    public async Task AddCity_ShouldCreateCity()
    {
        var request = BuildAddCityRequest("NEW");

        var response = await _client.PostAsJsonAsync("/api/cities", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var cityResponse = await response.Content.ReadFromJsonAsync<CityResponse>();
        cityResponse.Should().NotBeNull();
        cityResponse!.Code.Should().Be("NEW");
    }

    [TestMethod]
    public async Task DeleteCity_ShouldRemoveCity()
    {
        var code = "DEL";
        var addResponse = await _client.PostAsJsonAsync("/api/cities", BuildAddCityRequest(code));
        addResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var deleteResponse = await _client.DeleteAsync($"/api/cities/{code}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private static object BuildAddCityRequest(string code)
    {
        return new
        {
            Name = $"City_{code}",
            Code = code,
            MaxDailyTax = 60,
            SingleChargeMinutes = 60,
            IsActive = true,
            TaxRules = new[]
            {
                new
                {
                    StartTime = new TimeSpan(6,0,0),
                    EndTime = new TimeSpan(7,0,0),
                    Amount = 10m
                }
            },
        };
    }

    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}

