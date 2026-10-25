using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPharmacy.Api.Auth;
using MyPharmacy.Api.DTOs.Auth;
using MyPharmacy.Api.Models;
using MyPharmacy.Api.Services.Interfaces;

namespace MyPharmacy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IPharmacistService _pharmacistService;
    private readonly TokenService _tokenService;

    public AuthController(
        IPharmacistService pharmacistService,
        TokenService tokenService)
    {
        _pharmacistService = pharmacistService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var exists = await _pharmacistService.GetByEmailAsync(dto.Email);
        if (exists != null)
            return BadRequest("Email already exists");

        var pharmacist = new Pharmacist
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Address = dto.Address ?? string.Empty,
            Phone = dto.Phone ?? string.Empty,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _pharmacistService.CreateAsync(pharmacist);

        return Ok("Registration successful");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var pharmacist = await _pharmacistService.GetByEmailAsync(dto.Email);
        if (pharmacist == null)
            return Unauthorized("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, pharmacist.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = _tokenService.GenerateToken(pharmacist);

        return Ok(new { token });
    }
}
