using Microsoft.EntityFrameworkCore;
using SkyBooker.BookingService.Data;
using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Entities;
using SkyBooker.BookingService.Interfaces;

namespace SkyBooker.BookingService.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repo;
    private readonly BookingDbContext _context;

    public BookingService(IBookingRepository repo, BookingDbContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<Booking> CreateBooking(CreateBookingDto dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        try
        {
            var pnr = await GeneratePnr();

            var booking = new Booking
            {
                BookingId = Guid.NewGuid(),
                UserId = dto.UserId,
                FlightId = dto.FlightId,
                PnrCode = pnr,
                TripType = dto.TripType,

                BaseFare = dto.BaseFare,
                Taxes = dto.Taxes,
                TotalFare = dto.BaseFare + dto.Taxes,

                MealPreference = dto.MealPreference,
                LuggageKg = dto.LuggageKg,

                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,

                Status = "CONFIRMED"
            };

            await _repo.Add(booking);

            //  call SeatService -> HOLD seats
            // await _seatClient.HoldSeats(...);

            //  TODO: call FlightService -> DECREMENT seats
            // await _flightClient.DecrementSeats(dto.FlightId, count);

            await tx.CommitAsync();
            return booking;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task<Booking?> GetBookingById(Guid id)
        => await _repo.FindByBookingId(id);

    public async Task<Booking?> GetBookingByPnr(string pnr)
        => await _repo.FindByPnrCode(pnr);

    public async Task<List<Booking>> GetBookingsByUser(int userId)
        => await _repo.FindByUserId(userId);

    public async Task<List<Booking>> GetBookingsByFlight(int flightId)
        => await _repo.FindByFlightId(flightId);

    public async Task CancelBooking(Guid id)
    {
        var booking = await _repo.FindByBookingId(id);
        if (booking == null) return;

        booking.Status = "CANCELLED";

        // TODO: increment seats back
        // TODO: release seats

        await _repo.Update(booking);
    }

    public async Task UpdateStatus(Guid id, string status)
    {
        var booking = await _repo.FindByBookingId(id);
        if (booking == null) return;

        booking.Status = status;
        await _repo.Update(booking);
    }

    public Task<FareSummary> CalculateFare(decimal baseFare, decimal taxes, decimal ancillary)
    {
        var total = baseFare + taxes + ancillary;
        return Task.FromResult(new FareSummary(baseFare, taxes, ancillary, total));
    }

    public async Task AddAddOn(AddOnDto dto)
    {
        var booking = await _repo.FindByBookingId(dto.BookingId);
        if (booking == null) return;

        booking.LuggageKg += dto.ExtraLuggageKg;
        booking.MealPreference = dto.Meal;

        booking.TotalFare += (dto.ExtraLuggageKg * 100); // sample pricing

        await _repo.Update(booking);
    }

    public async Task<List<Booking>> GetUpcomingBookings(int userId)
    {
        var all = await _repo.FindByUserId(userId);
        return all.Where(b => b.BookedAt >= DateTime.UtcNow && b.Status == "CONFIRMED").ToList();
    }

    private async Task<string> GeneratePnr()
    {
        string pnr;
        do
        {
            pnr = Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
        while (await _repo.FindByPnrCode(pnr) != null);

        return pnr;
    }
}