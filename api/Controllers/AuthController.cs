using Microsoft.AspNetCore.Mvc;
using CursosPlatform.api.DTOs;
using CursosPlatform.api.Services;

namespace CursosPlatform.api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // POST /auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var resultado = await _authService.LoginAsync(request);

        if (resultado == null)
            return Unauthorized(new { message = "E-mail ou senha inválidos" });

        return Ok(resultado);
    }
}
