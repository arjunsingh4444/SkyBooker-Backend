using Microsoft.AspNetCore.Mvc;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Interfaces;

namespace SkyBooker.AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
        => Ok(await _service.Register(dto));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto dto)
        => Ok(await _service.Login(dto));

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _service.Logout();
        return Ok("Logged out");
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetProfile(int id)
        => Ok(await _service.GetUserById(id));

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UserDto dto)
    {
        await _service.UpdateProfile(dto);
        return Ok("Updated");
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(int userId, string newPassword)
    {
        await _service.ChangePassword(userId, newPassword);
        return Ok("Password changed");
    }

    [HttpDelete("deactivate/{userId}")]
    public async Task<IActionResult> Deactivate(int userId)
    {
        await _service.DeactivateAccount(userId);
        return Ok("Deactivated");
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
        => Ok(await _service.GetAllUsers());
}