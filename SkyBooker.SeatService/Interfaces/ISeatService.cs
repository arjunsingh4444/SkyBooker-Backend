using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Interfaces;

public interface ISeatService
{
    Task AddSeatsForFlight(int flightId, int rows, int seatsPerRow, string seatClass);

    Task<List<Seat>> GetAvailableSeats(int flightId);
    Task<List<Seat>> GetAvailableByClass(int flightId, string seatClass);
    Task<Seat?> GetSeatById(int id);

    Task HoldSeat(int seatId);
    Task ReleaseSeat(int seatId);
    Task ConfirmSeat(int seatId);

    Task<List<Seat>> GetSeatMap(int flightId);
    Task<int> CountAvailableByClass(int flightId, string seatClass);

    Task DeleteSeatsForFlight(int flightId);
}