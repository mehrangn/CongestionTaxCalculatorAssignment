using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Application.Services;
using CongestionTaxCalculator.Domain.Interfaces;
using CongestionTaxCalculator.Domain.Services;

namespace CongestionTaxCalculator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IVehicleFactory, VehicleFactory>();
        services.AddScoped<ITaxCalculationService, TaxCalculationService>();

        return services;
    }
}

