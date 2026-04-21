using Microsoft.EntityFrameworkCore;
using SkyBooker.SeatService.Entities;
using SkyBooker.SeatService.Interfaces;

namespace SkyBooker.SeatService.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repo;

    public SeatService(ISeatRepository repo)
    {
        _repo = repo;
    }

    public async Task AddSeatsForFlight(int flightId, int rows, int seatsPerRow, string seatClass)
    {
        var seats = new List<Seat>();

        for (int r = 1; r <= rows; r++)
        {
            for (int c = 0; c < seatsPerRow; c++)
            {
                var col = ((char)('A' + c)).ToString();
                seats.Add(new Seat
                {
                    FlightId = flightId,
                    SeatNumber = $"{r}{col}",
                    Row = r,
                    Column = col,
                    SeatClass = seatClass,
                    Status = "AVAILABLE"
                });
            }
        }

        await _repo.AddRange(seats);
    }

    public async Task<List<Seat>> GetAvailableSeats(int flightId)
        => await _repo.FindAvailableByFlightId(flightId);

    public async Task<List<Seat>> GetAvailableByClass(int flightId, string seatClass)
        => await _repo.FindByFlightIdAndSeatClass(flightId, seatClass);

    public async Task<Seat?> GetSeatById(int id)
        => await _repo.FindBySeatId(id);

    public async Task HoldSeat(int seatId)
    {
        var seat = await _repo.FindBySeatId(seatId);
        if (seat == null) throw new Exception("Seat not found");

        if (seat.Status != "AVAILABLE")
            throw new Exception("Seat not available");

        seat.Status = "HELD";
        seat.HeldSince = DateTime.UtcNow;

        try
        {
            await _repo.Update(seat);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("Seat already taken (409)");
        }
    }

    public async Task ReleaseSeat(int seatId)
    {
        var seat = await _repo.FindBySeatId(seatId);
        if (seat == null) return;

        seat.Status = "AVAILABLE";
        seat.HeldSince = null;

        await _repo.Update(seat);
    }

    public async Task ConfirmSeat(int seatId)
    {
        var seat = await _repo.FindBySeatId(seatId);
        if (seat == null) return;

        seat.Status = "CONFIRMED";
        await _repo.Update(seat);
    }

    public async Task<List<Seat>> GetSeatMap(int flightId)
        => await _repo.FindByFlightId(flightId);

    public async Task<int> CountAvailableByClass(int flightId, string seatClass)
        => await _repo.CountAvailableByClass(flightId, seatClass);

    public async Task DeleteSeatsForFlight(int flightId)
        => await _repo.DeleteByFlightId(flightId);
}