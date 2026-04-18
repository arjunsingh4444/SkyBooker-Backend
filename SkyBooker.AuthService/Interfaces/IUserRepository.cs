using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByEmail(string email);
    Task<User?> FindByUserId(int userId);
    Task<bool> ExistsByEmail(string email);
    Task<List<User>> FindAllByRole(string role);
    Task AddUser(User user);
    Task UpdateUser(User user);
}