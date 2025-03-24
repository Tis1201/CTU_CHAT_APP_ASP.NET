using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAspNetBackend.Data;
using MyAspNetBackend.DTOs;
using MyAspNetBackend.Models;
using MyAspNetBackend.Responses;
using MyAspNetBackend.Services;

namespace MyAspNetBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly ApplicationDbContext _context;

    public AuthController(JwtTokenService jwtTokenService, ApplicationDbContext context)
    {
        _jwtTokenService = jwtTokenService;
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == request.email);

        if (customer == null || !BCrypt.Net.BCrypt.Verify(request.password, customer.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        
        var tokens = _jwtTokenService.GenerateTokens(customer);

        
        customer.RefreshToken = tokens.refreshToken;
        customer.RefreshTokenExpires = DateTime.UtcNow.AddDays(7); 
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Success(new { customer.FullName, customer.Email, tokens.accessToken }));
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == request.email);

        if (existingCustomer != null)
        {
            return BadRequest("Email is already in use.");
        }

        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.password);

        var customer = new Customer
        {
            FullName = request.full_name,
            PhoneNumber = request.phone_number,  
            Email = request.email,
            Password = hashedPassword,
            Address = request.address,  
            Role = request.role,
            RegisteredAt = DateTime.UtcNow,
            RefreshToken = "", 
            RefreshTokenExpires = null 
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Success(new { customer.FullName, customer.Email }));
    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.RefreshToken == request.RefreshToken && c.RefreshTokenExpires > DateTime.UtcNow);

        if (customer == null)
        {
            return Unauthorized("Invalid or expired refresh token");
        }

        // Generate new access token
        var newAccessToken = _jwtTokenService.GenerateAccessToken(customer);

        return Ok(ApiResponse<object>.Success(new
        {
            accessToken = newAccessToken
        }));
    }

}