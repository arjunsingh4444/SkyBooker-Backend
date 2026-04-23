using Microsoft.EntityFrameworkCore;
using SkyBooker.AirlineService.Data;
using SkyBooker.AirlineService.Entities;
using SkyBooker.AirlineService.Interfaces;

namespace SkyBooker.AirlineService.Repositories;

public class AirlineRepository : IAirlineRepository
{
    private readonly AirlineDbContext _context;

    public AirlineRepository(AirlineDbContext context)
    {
        _context = context;
    }

    public async Task<Airline> CreateAirlineAsync(Airline airline)
    {
        _context.Airlines.Add(airline);
        await _context.SaveChangesAsync();
        return airline;
    }

    public async Task<Airport> CreateAirportAsync(Airport airport)
    {
        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
        return airport;
    }

    public async Task<Airport?> FindAirportByIataCodeAsync(string iataCode)
    {
        return await _context.Airports
            .Include(a => a.Airlines)
            .FirstOrDefaultAsync(a => a.IataCode == iataCode);
    }

    public async Task<IList<Airport>> FindAirportsByCityAsync(string city)
    {
        return await _context.Airports
            .Where(a => a.City == city)
            .ToListAsync();
    }

    public async Task<IList<Airport>> FindAirportsByCountryAsync(string country)
    {
        return await _context.Airports
            .Where(a => a.Country == country)
            .ToListAsync();
    }

    public async Task<Airline?> FindByAirlineIdAsync(int airlineId)
    {
        return await _context.Airlines
            .Include(a => a.Airports)
            .FirstOrDefaultAsync(a => a.AirlineId == airlineId);
    }

    public async Task<Airline?> FindByIataCodeAsync(string iataCode)
    {
        return await _context.Airlines
            .Include(a => a.Airports)
            .FirstOrDefaultAsync(a => a.IataCode == iataCode);
    }

    public async Task<IList<Airline>> FindByIsActiveAsync(bool isActive)
    {
        return await _context.Airlines
            .Where(a => a.IsActive == isActive)
            .ToListAsync();
    }

    public async Task<IList<Airport>> SearchAirportsAsync(string query)
    {
        var likeQuery = $"%{query}%";
        return await _context.Airports
            .Where(a => EF.Functions.Like(a.Name, likeQuery) || 
                        EF.Functions.Like(a.IataCode, likeQuery) || 
                        EF.Functions.Like(a.City, likeQuery))
            .Take(10)
            .ToListAsync();
    }

    public async Task UpdateAirlineAsync(Airline airline)
    {
        _context.Airlines.Update(airline);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAirportAsync(Airport airport)
    {
        _context.Airports.Update(airport);
        await _context.SaveChangesAsync();
    }
}
