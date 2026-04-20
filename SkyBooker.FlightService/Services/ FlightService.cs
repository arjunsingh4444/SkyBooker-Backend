using SkyBooker.FlightService.DTOs;
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

    public async Task AddFlight(FlightDto dto)
    {
        var f = new Flight
        {
            FlightNumber = dto.FlightNumber,
            AirlineId = dto.AirlineId,
            OriginAirportCode = dto.OriginAirportCode,
            DestinationAirportCode = dto.DestinationAirportCode,
            DepartureTime = dto.DepartureTime,
            ArrivalTime = dto.ArrivalTime,
            DurationMinutes = dto.DurationMinutes,
            Status = dto.Status,
            AircraftType = dto.AircraftType,
            TotalSeats = dto.TotalSeats,
            AvailableSeats = dto.AvailableSeats,
            BasePrice = dto.BasePrice
        };

        await _repo.Add(f);
    }

    public async Task<FlightDto?> GetFlightById(int id)
    {
        var f = await _repo.FindByFlightId(id);
        return f == null ? null : Map(f);
    }

    public async Task<FlightDto?> GetFlightByNumber(string number)
    {
        var f = await _repo.FindByFlightNumber(number);
        return f == null ? null : Map(f);
    }

    public async Task<List<FlightDto>> SearchFlights(string origin, string dest, DateTime date)
        => (await _repo.FindAvailableFlights(origin, dest, date)).Select(Map).ToList();

    // ROUND TRIP
    public async Task<Dictionary<string, IList<FlightDto>>> SearchRoundTrip(
        string origin, string dest, DateTime departDate, DateTime returnDate)
    {
        var outbound = await _repo.FindAvailableFlights(origin, dest, departDate);
        var ret = await _repo.FindAvailableFlights(dest, origin, returnDate);

        return new Dictionary<string, IList<FlightDto>>
        {
            { "outbound", outbound.Select(Map).ToList() },
            { "return", ret.Select(Map).ToList() }
        };
    }

    public async Task UpdateFlight(FlightDto dto)
    {
        var f = await _repo.FindByFlightId(dto.FlightId);
        if (f == null) return;

        f.BasePrice = dto.BasePrice;
        f.Status = dto.Status;

        await _repo.Update(f);
    }

    public async Task UpdateStatus(int flightId, string status)
    {
        var f = await _repo.FindByFlightId(flightId);
        if (f == null) return;

        f.Status = status;
        await _repo.Update(f);
    }

    public Task DecrementSeats(int flightId, int count)
        => _repo.DecrementSeats(flightId, count);

    public Task IncrementSeats(int flightId, int count)
        => _repo.IncrementSeats(flightId, count);

    public Task DeleteFlight(int id)
        => _repo.Delete(id);

    public async Task<List<FlightDto>> GetFlightsByAirline(int airlineId)
        => (await _repo.FindByAirlineId(airlineId)).Select(Map).ToList();

    private static FlightDto Map(Flight f) => new()
    {
        FlightId = f.FlightId,
        FlightNumber = f.FlightNumber,
        AirlineId = f.AirlineId,
        OriginAirportCode = f.OriginAirportCode,
        DestinationAirportCode = f.DestinationAirportCode,
        DepartureTime = f.DepartureTime,
        ArrivalTime = f.ArrivalTime,
        DurationMinutes = f.DurationMinutes,
        Status = f.Status,
        AircraftType = f.AircraftType,
        TotalSeats = f.TotalSeats,
        AvailableSeats = f.AvailableSeats,
        BasePrice = f.BasePrice
    };
}