using Microsoft.AspNetCore.Identity;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Entities;
using SkyBooker.AuthService.Interfaces;
using SkyBooker.AuthService.Helpers;

namespace SkyBooker.AuthService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    private readonly JwtHelper _jwt;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(IUserRepository repo, JwtHelper jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> Register(RegisterRequestDto dto)
    {
        if (await _repo.ExistsByEmail(dto.Email))
            throw new Exception("Email exists");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email
        };

        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        await _repo.AddUser(user);

        return new AuthResponseDto
        {
            Email = user.Email,
            Token = _jwt.GenerateToken(user.Email)
        };
    }

    public async Task<AuthResponseDto> Login(LoginRequestDto dto)
    {
        var user = await _repo.FindByEmail(dto.Email);

        if (user == null ||
            _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password)
            == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials");

        return new AuthResponseDto
        {
            Email = user.Email,
            Token = _jwt.GenerateToken(user.Email)
        };
    }

    public Task Logout() => Task.CompletedTask;

    public Task<string> RefreshToken(string email)
        => Task.FromResult(_jwt.GenerateToken(email));

    public async Task<UserDto?> GetUserById(int userId)
    {
        var user = await _repo.FindByUserId(userId);
        if (user == null) return null;

        return new UserDto
        {
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName
        };
    }

    public async Task UpdateProfile(UserDto dto)
    {
        var user = await _repo.FindByUserId(dto.UserId);
        if (user == null) return;

        user.FullName = dto.FullName;
        user.Email = dto.Email;

        await _repo.UpdateUser(user);
    }

    public async Task ChangePassword(int userId, string newPassword)
    {
        var user = await _repo.FindByUserId(userId);
        if (user == null) return;

        user.PasswordHash = _hasher.HashPassword(user, newPassword);

        await _repo.UpdateUser(user);
    }

    public async Task DeactivateAccount(int userId)
    {
        var user = await _repo.FindByUserId(userId);
        if (user == null) return;

        user.IsActive = false;

        await _repo.UpdateUser(user);
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await _repo.FindAllByRole("PASSENGER");

        return users.Select(u => new UserDto
        {
            UserId = u.UserId,
            Email = u.Email,
            FullName = u.FullName
        }).ToList();
    }
}