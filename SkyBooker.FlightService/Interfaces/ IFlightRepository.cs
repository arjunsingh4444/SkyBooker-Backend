using SkyBooker.FlightService.Entities;

namespace SkyBooker.FlightService.Interfaces;

public interface IFlightRepository
{
    Task<Flight?> FindByFlightNumber(string number);
    Task<Flight?> FindByFlightId(int id);
    Task<List<Flight>> FindByOriginDestDate(string origin, string dest, DateTime date);
    Task<List<Flight>> FindByAirlineId(int airlineId);
    Task<List<Flight>> FindByStatus(string status);
    Task<List<Flight>> FindAvailableFlights(string origin, string dest, DateTime date);
    Task<int> CountByAirlineId(int airlineId);

    Task Add(Flight flight);
    Task Update(Flight flight);
    Task Delete(int id);

    Task DecrementSeats(int flightId, int count);
    Task IncrementSeats(int flightId, int count);
}