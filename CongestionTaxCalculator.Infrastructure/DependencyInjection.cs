using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CongestionTaxCalculator.Infrastructure.Data;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Infrastructure.Repositories;

namespace CongestionTaxCalculator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ICityRepository, CityRepository>();

        return services;
    }
}

