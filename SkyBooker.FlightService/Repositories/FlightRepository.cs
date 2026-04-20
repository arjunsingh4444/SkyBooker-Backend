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

    public async Task<Flight?> FindByFlightNumber(string number)
        => await _context.Flights.FirstOrDefaultAsync(x => x.FlightNumber == number);

    public async Task<Flight?> FindByFlightId(int id)
        => await _context.Flights.FindAsync(id);

    public async Task<List<Flight>> FindByOriginDestDate(string origin, string dest, DateTime date)
        => await _context.Flights
            .Where(f => f.OriginAirportCode == origin &&
                        f.DestinationAirportCode == dest &&
                        f.DepartureTime.Date == date.Date)
            .ToListAsync();

    public async Task<List<Flight>> FindByAirlineId(int airlineId)
        => await _context.Flights.Where(x => x.AirlineId == airlineId).ToListAsync();

    public async Task<List<Flight>> FindByStatus(string status)
        => await _context.Flights.Where(x => x.Status == status).ToListAsync();

    public async Task<List<Flight>> FindAvailableFlights(string origin, string dest, DateTime date)
        => await _context.Flights
            .Where(f => f.OriginAirportCode == origin &&
                        f.DestinationAirportCode == dest &&
                        f.DepartureTime.Date == date.Date &&
                        f.AvailableSeats > 0)
            .ToListAsync();

    public async Task<int> CountByAirlineId(int airlineId)
        => await _context.Flights.CountAsync(x => x.AirlineId == airlineId);

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
        var f = await _context.Flights.FindAsync(id);
        if (f != null)
        {
            _context.Flights.Remove(f);
            await _context.SaveChangesAsync();
        }
    }

    // 🔥 ATOMIC UPDATE
    public async Task DecrementSeats(int flightId, int count)
    {
        await _context.Flights
            .Where(f => f.FlightId == flightId && f.AvailableSeats >= count)
            .ExecuteUpdateAsync(s => s.SetProperty(f => f.AvailableSeats, f => f.AvailableSeats - count));
    }

    public async Task IncrementSeats(int flightId, int count)
    {
        await _context.Flights
            .Where(f => f.FlightId == flightId)
            .ExecuteUpdateAsync(s => s.SetProperty(f => f.AvailableSeats, f => f.AvailableSeats + count));
    }
}