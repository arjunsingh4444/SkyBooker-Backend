using Microsoft.EntityFrameworkCore;
using SkyBooker.SeatService.Data;
using SkyBooker.SeatService.Entities;
using SkyBooker.SeatService.Interfaces;

namespace SkyBooker.SeatService.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly SeatDbContext _context;

    public SeatRepository(SeatDbContext context)
    {
        _context = context;
    }

    public async Task<Seat?> FindBySeatId(int id)
        => await _context.Seats.FindAsync(id);

    public async Task<Seat?> FindByFlightIdAndSeatNumber(int flightId, string seatNumber)
        => await _context.Seats.FirstOrDefaultAsync(s => s.FlightId == flightId && s.SeatNumber == seatNumber);

    public async Task<List<Seat>> FindByFlightId(int flightId)
        => await _context.Seats.Where(s => s.FlightId == flightId).ToListAsync();

    public async Task<List<Seat>> FindAvailableByFlightId(int flightId)
        => await _context.Seats.Where(s => s.FlightId == flightId && s.Status == "AVAILABLE").ToListAsync();

    public async Task<List<Seat>> FindByFlightIdAndSeatClass(int flightId, string seatClass)
        => await _context.Seats.Where(s => s.FlightId == flightId && s.SeatClass == seatClass).ToListAsync();

    public async Task<int> CountAvailableByClass(int flightId, string seatClass)
        => await _context.Seats.CountAsync(s => s.FlightId == flightId && s.SeatClass == seatClass && s.Status == "AVAILABLE");

    public async Task AddRange(List<Seat> seats)
    {
        _context.Seats.AddRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Seat seat)
    {
        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByFlightId(int flightId)
    {
        var seats = await FindByFlightId(flightId);
        _context.Seats.RemoveRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Seat>> FindExpiredHolds(DateTime threshold)
        => await _context.Seats
            .Where(s => s.Status == "HELD" && s.HeldSince < threshold)
            .ToListAsync();
}