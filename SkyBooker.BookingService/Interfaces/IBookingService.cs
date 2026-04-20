using SkyBooker.BookingService.Entities;
using SkyBooker.BookingService.DTOs;

namespace SkyBooker.BookingService.Interfaces;

public interface IBookingService
{
    Task<string> CreateBooking(CreateBookingDto dto);

    Task<Booking?> GetBookingById(Guid id);
    Task<Booking?> GetBookingByPnr(string pnr);
    Task<List<Booking>> GetBookingsByUser(int userId);

    Task CancelBooking(Guid bookingId);

    Task<FareSummary> CalculateFare(int seats, int luggageKg);
}