using Microsoft.EntityFrameworkCore;
using SkyBooker.BookingService.Data;
using SkyBooker.BookingService.Entities;
using SkyBooker.BookingService.Interfaces;

namespace SkyBooker.BookingService.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> FindByBookingId(Guid id)
        => await _context.Bookings.FindAsync(id);

    public async Task<Booking?> FindByPnrCode(string pnr)
        => await _context.Bookings.FirstOrDefaultAsync(b => b.PnrCode == pnr);

    public async Task<List<Booking>> FindByUserId(int userId)
        => await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();

    public async Task<List<Booking>> FindByFlightId(int flightId)
        => await _context.Bookings.Where(b => b.FlightId == flightId).ToListAsync();

    public async Task<List<Booking>> FindByStatus(string status)
        => await _context.Bookings.Where(b => b.Status == status).ToListAsync();

    public async Task<int> CountByFlightIdAndStatus(int flightId, string status)
        => await _context.Bookings.CountAsync(b => b.FlightId == flightId && b.Status == status);

    public async Task<List<Booking>> FindByUserIdAndStatus(int userId, string status)
        => await _context.Bookings.Where(b => b.UserId == userId && b.Status == status).ToListAsync();

    public async Task Add(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }
}