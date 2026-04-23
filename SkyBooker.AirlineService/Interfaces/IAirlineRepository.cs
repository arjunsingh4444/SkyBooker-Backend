using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Interfaces;

public interface IAirlineRepository
{
    Task<Airline?> FindByAirlineIdAsync(int airlineId);
    Task<Airline?> FindByIataCodeAsync(string iataCode);
    Task<IList<Airline>> FindByIsActiveAsync(bool isActive);
    Task<Airline> CreateAirlineAsync(Airline airline);
    Task UpdateAirlineAsync(Airline airline);

    Task<Airport?> FindAirportByIataCodeAsync(string iataCode);
    Task<IList<Airport>> FindAirportsByCityAsync(string city);
    Task<IList<Airport>> FindAirportsByCountryAsync(string country);
    Task<IList<Airport>> SearchAirportsAsync(string query);
    Task<Airport> CreateAirportAsync(Airport airport);
    Task UpdateAirportAsync(Airport airport);
}
