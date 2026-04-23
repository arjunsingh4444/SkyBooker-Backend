using SkyBooker.FlightService.Entities;
using SkyBooker.FlightService.Interfaces;

namespace SkyBooker.FlightService.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _repo;

    public FlightService(IFlightRepository repo)
    {
        _repo = repo;
    }

    public async Task AddFlight(Flight flight)
    {
        flight.AvailableSeats = flight.TotalSeats;
        await _repo.Add(flight);
    }

    public async Task<Flight?> GetFlightById(int id)
        => await _repo.FindByFlightId(id);

    public async Task<Flight?> GetFlightByNumber(string flightNumber)
        => await _repo.FindByFlightNumber(flightNumber);

    public async Task<List<Flight>> SearchFlights(string origin, string destination, DateTime date)
        => await _repo.FindAvailableFlights(origin, destination, date);

    public async Task<Dictionary<string, IList<Flight>>> SearchRoundTrip(string origin, string destination, DateTime depart, DateTime ret)
    {
        var outbound = await _repo.FindAvailableFlights(origin, destination, depart);
        var inbound = await _repo.FindAvailableFlights(destination, origin, ret);

        return new Dictionary<string, IList<Flight>>
        {
            { "outbound", outbound },
            { "return", inbound }
        };
    }

    public async Task UpdateFlight(Flight flight)
        => await _repo.Update(flight);

    public async Task UpdateStatus(int flightId, string status)
    {
        var flight = await _repo.FindByFlightId(flightId);
        if (flight == null) return;

        flight.Status = status;
        await _repo.Update(flight);
    }

    public async Task DecrementSeats(int flightId, int count)
    {
        var flight = await _repo.FindByFlightId(flightId);
        if (flight == null) return;

        flight.AvailableSeats -= count;
        await _repo.Update(flight);
    }

    public async Task IncrementSeats(int flightId, int count)
    {
        var flight = await _repo.FindByFlightId(flightId);
        if (flight == null) return;

        flight.AvailableSeats += count;
        await _repo.Update(flight);
    }

    public async Task DeleteFlight(int id)
        => await _repo.Delete(id);

    public async Task<List<Flight>> GetFlightsByAirline(int airlineId)
        => await _repo.FindByAirlineId(airlineId);
}