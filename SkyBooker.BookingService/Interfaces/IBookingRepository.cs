using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> FindByBookingId(Guid id);
    Task<Booking?> FindByPnrCode(string pnr);

    Task<List<Booking>> FindByUserId(int userId);
    Task<List<Booking>> FindByFlightId(int flightId);
    Task<List<Booking>> FindByStatus(string status);

    Task<int> CountByFlightIdAndStatus(int flightId, string status);
    Task<List<Booking>> FindByUserIdAndStatus(int userId, string status);

    Task Add(Booking booking);
    Task Update(Booking booking);
}