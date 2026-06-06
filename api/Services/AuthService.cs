using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CursosPlatform.api.Data;
using CursosPlatform.api.DTOs;
using CursosPlatform.api.Models;

namespace CursosPlatform.api.Services;

public class AuthService
{
    private readonly AppDbContext   _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config  = config;
    }

    // Valida credenciais e retorna o DTO com token JWT; retorna null se inválido
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        // Verifica se o usuário existe e se a senha corresponde ao hash armazenado
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return null;

        return new LoginResponseDto
        {
            Id    = user.Id,
            Token = GerarToken(user),
            Name  = user.Name,
            Email = user.Email,
            Role  = user.Role
        };
    }

    private string GerarToken(User user)
    {
        var chave       = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        // Claims são as informações embutidas no payload do token JWT
        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("email",  user.Email),
            new Claim("role",   user.Role),
            new Claim("name",   user.Name)
        };

        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(8),
            signingCredentials: credenciais
        );

        // Serializa o objeto JwtSecurityToken para a string compacta (header.payload.signature)
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
