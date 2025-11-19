using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Application.Interfaces;

public interface ICityRepository
{
    Task<City?> GetByCodeAsync(string code, CancellationToken cancellationToken);
    Task<ICollection<TaxRule>> GetTaxRulesByCityIdAsync(int cityId, CancellationToken cancellationToken);
    Task<ICollection<PublicHoliday>> GetPublicHolidaysAsync(CancellationToken cancellationToken);
    Task<bool> CityCodeExistsAsync(string code, CancellationToken cancellationToken);
    Task<City> AddCityAsync(City city, IEnumerable<TaxRule> taxRules, CancellationToken cancellationToken);
    Task<bool> DeleteCityByCodeAsync(string code, CancellationToken cancellationToken);
}

