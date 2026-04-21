using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Interfaces;

public interface IBookingService
{
    Task<Booking> CreateBooking(CreateBookingDto dto);

    Task<Booking?> GetBookingById(Guid id);
    Task<Booking?> GetBookingByPnr(string pnr);

    Task<List<Booking>> GetBookingsByUser(int userId);
    Task<List<Booking>> GetBookingsByFlight(int flightId);

    Task CancelBooking(Guid id);
    Task UpdateStatus(Guid id, string status);

    Task<FareSummary> CalculateFare(decimal baseFare, decimal taxes, decimal ancillary);

    Task AddAddOn(AddOnDto dto);

    Task<List<Booking>> GetUpcomingBookings(int userId);
}