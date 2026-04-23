using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Interfaces;

public interface IAirlineService
{
    // Airline Methods
    Task<Airline> CreateAirlineAsync(Airline airline);
    Task<Airline?> GetAirlineByIdAsync(int airlineId);
    Task<Airline?> GetAirlineByIataAsync(string iataCode);
    Task<IList<Airline>> GetAllAirlinesAsync();
    Task<Airline> UpdateAirlineAsync(int airlineId, Airline airline);
    Task DeactivateAirlineAsync(int airlineId);

    // Airport Methods
    Task<Airport> CreateAirportAsync(Airport airport);
    Task<Airport?> GetAirportByIataAsync(string iataCode);
    Task<IList<Airport>> SearchAirportsAsync(string query);
    Task<IList<Airport>> GetAirportsByCityAsync(string city);
    Task<Airport> UpdateAirportAsync(int airportId, Airport airport);
}
