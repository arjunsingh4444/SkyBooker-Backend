using SkyBooker.PassengerService.DTOs;
using SkyBooker.PassengerService.Entities;
using SkyBooker.PassengerService.Interfaces;

namespace SkyBooker.PassengerService.Services;

public class PassengerService : IPassengerService
{
    private readonly IPassengerRepository _repo;

    public PassengerService(IPassengerRepository repo)
    {
        _repo = repo;
    }

    public async Task Add(PassengerDto dto)
    {
        var p = new Passenger
        {
            UserId = dto.UserId,
            FullName = dto.FullName,
            Age = dto.Age,
            Gender = dto.Gender,
            PassportNumber = dto.PassportNumber,
            Nationality = dto.Nationality
        };

        await _repo.Add(p);
    }

    public async Task Update(int id, PassengerDto dto)
    {
        var p = await _repo.GetById(id);
        if (p == null) throw new Exception("Not found");

        p.FullName = dto.FullName;
        p.Age = dto.Age;
        p.Gender = dto.Gender;
        p.PassportNumber = dto.PassportNumber;
        p.Nationality = dto.Nationality;

        await _repo.Update(p);
    }

    public async Task AssignSeat(AssignSeatDto dto)
    {
        var p = await _repo.GetById(dto.PassengerId);
        if (p == null) throw new Exception("Not found");

        p.SeatId = dto.SeatId;
        await _repo.Update(p);
    }

    public async Task<Passenger?> GetById(int id) => await _repo.GetById(id);
    public async Task<List<Passenger>> GetByUser(int userId) => await _repo.GetByUserId(userId);
    public async Task<List<Passenger>> GetByBooking(int bookingId) => await _repo.GetByBookingId(bookingId);
    public async Task<Passenger?> GetByPassport(string passport) => await _repo.GetByPassport(passport);

    public async Task<int> Count(int bookingId) => await _repo.CountByBooking(bookingId);

    public async Task Delete(int id) => await _repo.Delete(id);
}