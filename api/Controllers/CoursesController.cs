using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CursosPlatform.api.DTOs;
using CursosPlatform.api.Services;

namespace CursosPlatform.api.Controllers;

[ApiController]
[Route("courses")]
[Authorize] // Todos os endpoints exigem token JWT válido
public class CoursesController : ControllerBase
{
    private readonly CourseService     _courseService;
    private readonly EnrollmentService _enrollmentService;

    public CoursesController(CourseService courseService, EnrollmentService enrollmentService)
    {
        _courseService     = courseService;
        _enrollmentService = enrollmentService;
    }

    // GET /courses — acessível por Admin e Student
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cursos = await _courseService.GetAllAsync();
        return Ok(cursos);
    }

    // POST /courses — somente Admin pode criar cursos
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        var curso = await _courseService.CreateAsync(dto);
        return Created($"/courses/{curso.Id}", curso);
    }

    // POST /courses/{id}/enrollments — somente Student pode se inscrever
    [HttpPost("{id}/enrollments")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> Enroll(int id)
    {
        // Lê o ID do usuário logado a partir do claim "userId" embutido no token JWT
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim == null)
            return Unauthorized(new { message = "Token inválido" });

        var userId = int.Parse(userIdClaim);

        var (enrollment, error) = await _enrollmentService.EnrollAsync(id, userId);

        if (error != null)
        {
            // 404 se o curso não existe, 400 se já está inscrito
            if (error.Contains("não encontrado"))
                return NotFound(new { message = error });

            return BadRequest(new { message = error });
        }

        return Created($"/courses/{id}/enrollments", enrollment);
    }
}
