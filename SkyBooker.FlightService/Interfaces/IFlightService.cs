using SkyBooker.FlightService.Entities;

namespace SkyBooker.FlightService.Interfaces;

public interface IFlightService
{
    Task AddFlight(Flight flight);
    Task<Flight?> GetFlightById(int id);
    Task<Flight?> GetFlightByNumber(string flightNumber);

    Task<List<Flight>> SearchFlights(string origin, string destination, DateTime date);
    Task<Dictionary<string, IList<Flight>>> SearchRoundTrip(string origin, string destination, DateTime depart, DateTime ret);

    Task UpdateFlight(Flight flight);
    Task UpdateStatus(int flightId, string status);

    Task DecrementSeats(int flightId, int count);
    Task IncrementSeats(int flightId, int count);

    Task DeleteFlight(int id);
    Task<List<Flight>> GetFlightsByAirline(int airlineId);
}