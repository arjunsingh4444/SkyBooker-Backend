using SkyBooker.AuthService.DTOs;

namespace SkyBooker.AuthService.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> Register(RegisterRequestDto dto);
    Task<AuthResponseDto> Login(LoginRequestDto dto);
    Task Logout();
    Task<string> RefreshToken(string email);
    Task<UserDto?> GetUserById(int userId);
    Task UpdateProfile(UserDto dto);
    Task ChangePassword(int userId, string newPassword);
    Task DeactivateAccount(int userId);
    Task<List<UserDto>> GetAllUsers();
}