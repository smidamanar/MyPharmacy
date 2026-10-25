using Microsoft.IdentityModel.Tokens;
using MyPharmacy.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyPharmacy.Api.Auth;

public class TokenService
{
    private readonly JwtSettings _settings;

    public TokenService(JwtSettings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(Pharmacist pharmacist)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, pharmacist.PharmacistID.ToString()),
            new Claim(ClaimTypes.Email, pharmacist.Email),
            new Claim(ClaimTypes.Name, pharmacist.FullName),
            new Claim(ClaimTypes.Role, pharmacist.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.Key)
        );

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
