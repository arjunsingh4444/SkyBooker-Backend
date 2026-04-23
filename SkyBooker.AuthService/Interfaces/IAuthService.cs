using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Interfaces;

public interface IAuthService
{
    Task Register(RegisterDto dto);
    Task<string> Login(LoginDto dto);
    Task<User?> GetProfile(int id);
    Task UpdateProfile(UpdateProfileDto dto);
    Task ChangePassword(ChangePasswordDto dto);
    Task Deactivate(int id);
    Task<List<User>> GetUsers();
}