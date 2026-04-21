using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Interfaces;

public interface IPassengerRepository
{
    Task Add(Passenger passenger);
    Task Update(Passenger passenger);
    Task Delete(int id);

    Task<Passenger?> GetById(int id);
    Task<List<Passenger>> GetByUserId(int userId);
    Task<List<Passenger>> GetByBookingId(int bookingId);
    Task<Passenger?> GetByPassport(string passport);

    Task<int> CountByBooking(int bookingId);
}