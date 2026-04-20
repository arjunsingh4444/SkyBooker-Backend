using SkyBooker.FlightService.DTOs;

namespace SkyBooker.FlightService.Interfaces;

public interface IFlightService
{
    Task AddFlight(FlightDto dto);
    Task<FlightDto?> GetFlightById(int id);
    Task<FlightDto?> GetFlightByNumber(string number);

    Task<List<FlightDto>> SearchFlights(string origin, string dest, DateTime date);

    Task<Dictionary<string, IList<FlightDto>>> SearchRoundTrip(
        string origin, string dest, DateTime departDate, DateTime returnDate);

    Task UpdateFlight(FlightDto dto);
    Task UpdateStatus(int flightId, string status);

    Task DecrementSeats(int flightId, int count);
    Task IncrementSeats(int flightId, int count);

    Task DeleteFlight(int id);
    Task<List<FlightDto>> GetFlightsByAirline(int airlineId);
}