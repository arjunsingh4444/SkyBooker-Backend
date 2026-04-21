using Microsoft.EntityFrameworkCore;
using SkyBooker.PassengerService.Data;
using SkyBooker.PassengerService.Entities;
using SkyBooker.PassengerService.Interfaces;

namespace SkyBooker.PassengerService.Repositories;

public class PassengerRepository : IPassengerRepository
{
    private readonly PassengerDbContext _context;

    public PassengerRepository(PassengerDbContext context)
    {
        _context = context;
    }

    public async Task Add(Passenger passenger)
    {
        _context.Passengers.Add(passenger);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Passenger passenger)
    {
        _context.Passengers.Update(passenger);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var p = await _context.Passengers.FindAsync(id);
        if (p != null)
        {
            _context.Passengers.Remove(p);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Passenger?> GetById(int id)
        => await _context.Passengers.FindAsync(id);

    public async Task<List<Passenger>> GetByUserId(int userId)
        => await _context.Passengers.Where(x => x.UserId == userId).ToListAsync();

    public async Task<List<Passenger>> GetByBookingId(int bookingId)
        => await _context.Passengers.Where(x => x.BookingId == bookingId).ToListAsync();

    public async Task<Passenger?> GetByPassport(string passport)
        => await _context.Passengers.FirstOrDefaultAsync(x => x.PassportNumber == passport);

    public async Task<int> CountByBooking(int bookingId)
        => await _context.Passengers.CountAsync(x => x.BookingId == bookingId);
}