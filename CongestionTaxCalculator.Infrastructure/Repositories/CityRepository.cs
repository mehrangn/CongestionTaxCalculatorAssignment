using Microsoft.EntityFrameworkCore;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Infrastructure.Data;

namespace CongestionTaxCalculator.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await _context.Cities
            .FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<ICollection<TaxRule>> GetTaxRulesByCityIdAsync(int cityId, CancellationToken cancellationToken)
    {
        return await _context.TaxRules
            .Where(tr => tr.CityId == cityId)
            .OrderBy(tr => tr.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<PublicHoliday>> GetPublicHolidaysAsync(CancellationToken cancellationToken)
    {
        return await _context.PublicHolidays
            .OrderBy(ph => ph.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CityCodeExistsAsync(string code, CancellationToken cancellationToken)
    {
        return await _context.Cities.AnyAsync(c => c.Code == code, cancellationToken);
    }

    public async Task<City> AddCityAsync(City city, IEnumerable<TaxRule> taxRules, CancellationToken cancellationToken)
    {
        await _context.Cities.AddAsync(city, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var taxRuleList = taxRules?.ToList() ?? new List<TaxRule>();
        foreach (var rule in taxRuleList)
        {
            rule.CityId = city.Id;
        }

        if (taxRuleList.Count > 0)
        {
            await _context.TaxRules.AddRangeAsync(taxRuleList, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return city;
    }

    public async Task<bool> DeleteCityByCodeAsync(string code, CancellationToken cancellationToken)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(c => c.Code == code, cancellationToken);
        if (city == null)
        {
            return false;
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

