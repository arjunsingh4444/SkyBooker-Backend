using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByEmail(string email);
    Task<User?> FindByUserId(int id);
    Task<List<User>> GetAll();
    Task Add(User user);
    Task Update(User user);
}