using Microsoft.EntityFrameworkCore;
using CursosPlatform.api.Data;
using CursosPlatform.api.DTOs;
using CursosPlatform.api.Models;

namespace CursosPlatform.api.Services;

public class CourseService
{
    private readonly AppDbContext _context;

    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    // Retorna todos os cursos como DTO (sem expor o modelo interno)
    public async Task<List<CourseDto>> GetAllAsync()
    {
        return await _context.Courses
            .Select(c => new CourseDto
            {
                Id              = c.Id,
                Title           = c.Title,
                Description     = c.Description,
                Workload        = c.Workload,
                // Conta quantas inscrições cada curso tem via propriedade de navegação
                EnrollmentCount = c.Enrollments.Count
            })
            .ToListAsync();
    }

    // Busca um curso pelo ID; retorna null se não encontrado
    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return null;

        return new CourseDto
        {
            Id          = course.Id,
            Title       = course.Title,
            Description = course.Description,
            Workload    = course.Workload
        };
    }

    // Cria um novo curso e retorna o DTO com o ID gerado pelo banco
    public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Title       = dto.Title,
            Description = dto.Description,
            Workload    = dto.Workload,
            CreatedAt   = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return new CourseDto
        {
            Id          = course.Id,
            Title       = course.Title,
            Description = course.Description,
            Workload    = course.Workload
        };
    }
}
