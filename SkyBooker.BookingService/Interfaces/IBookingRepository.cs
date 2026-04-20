using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Interfaces;

public interface IBookingRepository
{
    Task Add(Booking booking);
    Task<Booking?> GetById(Guid id);
    Task<Booking?> GetByPnr(string pnr);
    Task<List<Booking>> GetByUser(int userId);
    Task Update(Booking booking);
}