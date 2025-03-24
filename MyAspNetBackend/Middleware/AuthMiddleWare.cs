
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MyAspNetBackend.Data;
using MyAspNetBackend.Models;

namespace MyAspNetBackend.Middleware;
public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;

    public AuthMiddleware(RequestDelegate next, IConfiguration configuration, ApplicationDbContext  dbContext)
    {
        _next = next;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var refreshToken = context.Request.Headers["x-refresh-token"].FirstOrDefault();

        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Access denied. No token provided.");
            return;
        }

        var accessToken = authHeader.Split(" ")[1];

        try
        {
            // Verify the access token
            var principal = ValidateAccessToken(accessToken);
            if (principal != null)
            {
                context.User = principal;
                await _next(context);
                return;
            }
        }
        catch (Exception)
        {
            // Handle invalid token cases (TokenExpiredError)
        }

        // If access token expired, try to refresh using refresh token
        if (refreshToken == null)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Refresh token is required.");
            return;
        }

        var user = await _dbContext.Customers
            .Where(c => c.RefreshToken == refreshToken && c.RefreshTokenExpires > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid or expired refresh token.");
            return;
        }

        // Generate new access token
        var newAccessToken = GenerateAccessToken(user);
        context.Response.Headers["x-access-token"] = newAccessToken;

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.CustomerId.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        }, "Bearer"));

        context.User = claims;
        await _next(context);
    }

    private ClaimsPrincipal ValidateAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        try
        {
            var principal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Token expiry check
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private string GenerateAccessToken(Customer user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.CustomerId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddMinutes(15), // Token expires in 15 minutes
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
