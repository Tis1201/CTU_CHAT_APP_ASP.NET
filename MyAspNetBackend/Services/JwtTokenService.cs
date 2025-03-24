using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAspNetBackend.Models;
using MyAspNetBackend.Settings;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MyAspNetBackend.Services;
    
public class JwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    // Tạo Access Token
    public string GenerateAccessToken(Customer customer)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, customer.CustomerId.ToString()),
            new Claim("customer_id", customer.CustomerId.ToString()), // 👈 thêm dòng này
            new Claim(JwtRegisteredClaimNames.Email, customer.Email),
            new Claim("full_name", customer.FullName.ToString()),
            new Claim("role", customer.Role ? "admin" : "user"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), // Short expiration time for access token
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Tạo Refresh Token
    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();  
    }

    
    public (string accessToken, string refreshToken) GenerateTokens(Customer customer)
    {
        var accessToken = GenerateAccessToken(customer);
        var refreshToken = GenerateRefreshToken();
        return (accessToken, refreshToken);
    }
}
