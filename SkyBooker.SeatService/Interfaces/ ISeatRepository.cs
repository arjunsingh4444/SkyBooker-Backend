using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Interfaces;

public interface ISeatRepository
{
    Task<Seat?> FindBySeatId(int id);
    Task<Seat?> FindByFlightIdAndSeatNumber(int flightId, string seatNumber);

    Task<List<Seat>> FindByFlightId(int flightId);
    Task<List<Seat>> FindAvailableByFlightId(int flightId);
    Task<List<Seat>> FindByFlightIdAndSeatClass(int flightId, string seatClass);

    Task<int> CountAvailableByClass(int flightId, string seatClass);

    Task AddRange(List<Seat> seats);
    Task Update(Seat seat);
    Task DeleteByFlightId(int flightId);

    Task<List<Seat>> FindExpiredHolds(DateTime threshold);
}