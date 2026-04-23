using SkyBooker.FlightService.Entities;

namespace SkyBooker.FlightService.Interfaces;

public interface IFlightRepository
{
    Task<Flight?> FindByFlightId(int id);
    Task<Flight?> FindByFlightNumber(string flightNumber);

    Task<List<Flight>> FindByOriginDestDate(string origin, string destination, DateTime date);
    Task<List<Flight>> FindByAirlineId(int airlineId);
    Task<List<Flight>> FindByStatus(string status);

    Task<List<Flight>> FindAvailableFlights(string origin, string destination, DateTime date);

    Task<int> CountByAirlineId(int airlineId);

    Task Add(Flight flight);
    Task Update(Flight flight);
    Task Delete(int id);
}