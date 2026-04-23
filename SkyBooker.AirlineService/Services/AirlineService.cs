using SkyBooker.AirlineService.Entities;
using SkyBooker.AirlineService.Interfaces;

namespace SkyBooker.AirlineService.Services;

public class AirlineService : IAirlineService
{
    private readonly IAirlineRepository _repository;

    public AirlineService(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airline> CreateAirlineAsync(Airline airline)
    {
        return await _repository.CreateAirlineAsync(airline);
    }

    public async Task<Airport> CreateAirportAsync(Airport airport)
    {
        return await _repository.CreateAirportAsync(airport);
    }

    public async Task DeactivateAirlineAsync(int airlineId)
    {
        var airline = await _repository.FindByAirlineIdAsync(airlineId);
        if (airline != null)
        {
            airline.IsActive = false;
            await _repository.UpdateAirlineAsync(airline);
        }
    }

    public async Task<Airline?> GetAirlineByIdAsync(int airlineId)
    {
        return await _repository.FindByAirlineIdAsync(airlineId);
    }

    public async Task<Airline?> GetAirlineByIataAsync(string iataCode)
    {
        return await _repository.FindByIataCodeAsync(iataCode);
    }

    public async Task<IList<Airport>> GetAirportsByCityAsync(string city)
    {
        return await _repository.FindAirportsByCityAsync(city);
    }

    public async Task<Airport?> GetAirportByIataAsync(string iataCode)
    {
        return await _repository.FindAirportByIataCodeAsync(iataCode);
    }

    public async Task<IList<Airline>> GetAllAirlinesAsync()
    {
        // For simplification, getting all active airlines
        return await _repository.FindByIsActiveAsync(true);
    }

    public async Task<IList<Airport>> SearchAirportsAsync(string query)
    {
        return await _repository.SearchAirportsAsync(query);
    }

    public async Task<Airline> UpdateAirlineAsync(int airlineId, Airline airlineIn)
    {
        var airline = await _repository.FindByAirlineIdAsync(airlineId);
        if (airline != null)
        {
            airline.Name = airlineIn.Name;
            airline.IataCode = airlineIn.IataCode;
            airline.IcaoCode = airlineIn.IcaoCode;
            airline.LogoUrl = airlineIn.LogoUrl;
            airline.Country = airlineIn.Country;
            airline.ContactEmail = airlineIn.ContactEmail;
            airline.ContactPhone = airlineIn.ContactPhone;
            airline.IsActive = airlineIn.IsActive;

            await _repository.UpdateAirlineAsync(airline);
            return airline;
        }
        throw new KeyNotFoundException("Airline not found");
    }

    public async Task<Airport> UpdateAirportAsync(int airportId, Airport airportIn)
    {
        var airport = await _repository.FindAirportByIataCodeAsync(airportIn.IataCode);
        if (airport != null && airport.AirportId == airportId)
        {
            airport.Name = airportIn.Name;
            airport.City = airportIn.City;
            airport.Country = airportIn.Country;
            airport.Latitude = airportIn.Latitude;
            airport.Longitude = airportIn.Longitude;
            airport.Timezone = airportIn.Timezone;

            await _repository.UpdateAirportAsync(airport);
            return airport;
        }
        throw new KeyNotFoundException("Airport not found");
    }
}
