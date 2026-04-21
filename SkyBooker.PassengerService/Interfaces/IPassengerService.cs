using SkyBooker.PassengerService.DTOs;
using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Interfaces;

public interface IPassengerService
{
    Task Add(PassengerDto dto);
    Task Update(int id, PassengerDto dto);
    Task AssignSeat(AssignSeatDto dto);

    Task<Passenger?> GetById(int id);
    Task<List<Passenger>> GetByUser(int userId);
    Task<List<Passenger>> GetByBooking(int bookingId);
    Task<Passenger?> GetByPassport(string passport);

    Task<int> Count(int bookingId);
    Task Delete(int id);
}