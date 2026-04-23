using Microsoft.EntityFrameworkCore;
using SkyBooker.FlightService.Data;
using SkyBooker.FlightService.Entities;
using SkyBooker.FlightService.Interfaces;

namespace SkyBooker.FlightService.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly FlightDbContext _context;

    public FlightRepository(FlightDbContext context)
    {
        _context = context;
    }

    public async Task<Flight?> FindByFlightId(int id)
        => await _context.Flights.FindAsync(id);

    public async Task<Flight?> FindByFlightNumber(string flightNumber)
        => await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber);

    public async Task<List<Flight>> FindByOriginDestDate(string origin, string destination, DateTime date)
        => await _context.Flights
            .Where(f => f.OriginAirportCode == origin &&
                        f.DestinationAirportCode == destination &&
                        f.DepartureTime.Date == date.Date)
            .ToListAsync();

    public async Task<List<Flight>> FindByAirlineId(int airlineId)
        => await _context.Flights.Where(f => f.AirlineId == airlineId).ToListAsync();

    public async Task<List<Flight>> FindByStatus(string status)
        => await _context.Flights.Where(f => f.Status == status).ToListAsync();

    public async Task<List<Flight>> FindAvailableFlights(string origin, string destination, DateTime date)
        => await _context.Flights
            .Where(f => f.OriginAirportCode == origin &&
                        f.DestinationAirportCode == destination &&
                        f.DepartureTime.Date == date.Date &&
                        f.AvailableSeats > 0)
            .ToListAsync();

    public async Task<int> CountByAirlineId(int airlineId)
        => await _context.Flights.CountAsync(f => f.AirlineId == airlineId);

    public async Task Add(Flight flight)
    {
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Flight flight)
    {
        _context.Flights.Update(flight);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var flight = await FindByFlightId(id);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }
}