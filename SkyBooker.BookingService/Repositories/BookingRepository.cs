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

    public async Task Add(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<Booking?> GetById(Guid id)
        => await _context.Bookings.FindAsync(id);

    public async Task<Booking?> GetByPnr(string pnr)
        => await _context.Bookings.FirstOrDefaultAsync(x => x.PnrCode == pnr);

    public async Task<List<Booking>> GetByUser(int userId)
        => await _context.Bookings.Where(x => x.UserId == userId).ToListAsync();

    public async Task Update(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }
}