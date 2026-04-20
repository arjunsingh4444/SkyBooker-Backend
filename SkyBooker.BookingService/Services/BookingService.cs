using SkyBooker.BookingService.Data;
using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Entities;
using SkyBooker.BookingService.Interfaces;

namespace SkyBooker.BookingService.Services;

public class BookingService : IBookingService
{
    private readonly BookingDbContext _context;
    private readonly IBookingRepository _repo;

    public BookingService(BookingDbContext context, IBookingRepository repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task<string> CreateBooking(CreateBookingDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var fare = await CalculateFare(dto.Seats, dto.LuggageKg);
        var pnr = Guid.NewGuid().ToString("N")[..6].ToUpper();

        var booking = new Booking
        {
            UserId = dto.UserId,
            FlightId = dto.FlightId,
            PnrCode = pnr,
            BaseFare = fare.BaseFare,
            Taxes = fare.Taxes,
            TotalFare = fare.TotalFare,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            LuggageKg = dto.LuggageKg
        };

        await _repo.Add(booking);

        await transaction.CommitAsync();

        return pnr;
    }

    public async Task<Booking?> GetBookingById(Guid id)
        => await _repo.GetById(id);

    public async Task<Booking?> GetBookingByPnr(string pnr)
        => await _repo.GetByPnr(pnr);

    public async Task<List<Booking>> GetBookingsByUser(int userId)
        => await _repo.GetByUser(userId);

    public async Task CancelBooking(Guid bookingId)
    {
        var booking = await _repo.GetById(bookingId);
        if (booking == null) return;

        booking.Status = "CANCELLED";
        await _repo.Update(booking);
    }

    public Task<FareSummary> CalculateFare(int seats, int luggageKg)
    {
        decimal baseFare = seats * 5000;
        decimal taxes = baseFare * 0.18m;
        decimal luggage = luggageKg * 50;

        return Task.FromResult(new FareSummary(
            baseFare,
            taxes,
            luggage,
            baseFare + taxes + luggage
        ));
    }
}