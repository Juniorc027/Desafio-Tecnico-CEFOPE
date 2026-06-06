using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CursosPlatform.api.Services;

namespace CursosPlatform.api.Controllers;

[ApiController]
[Route("enrollments")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentsController(EnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    // GET /enrollments — lista todas as inscrições (somente Admin)
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll()
    {
        var inscricoes = await _enrollmentService.GetAllAsync();
        return Ok(inscricoes);
    }

    // GET /enrollments/mine — lista as inscrições do aluno logado (somente Student)
    [HttpGet("mine")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetMine()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var inscricoes = await _enrollmentService.GetByUserIdAsync(userId);
        return Ok(inscricoes);
    }
}
