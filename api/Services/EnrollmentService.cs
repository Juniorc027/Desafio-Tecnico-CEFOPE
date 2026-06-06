using Microsoft.EntityFrameworkCore;
using CursosPlatform.api.Data;
using CursosPlatform.api.DTOs;
using CursosPlatform.api.Models;

namespace CursosPlatform.api.Services;

public class EnrollmentService
{
    private readonly AppDbContext _context;

    public EnrollmentService(AppDbContext context)
    {
        _context = context;
    }

    // Retorna todas as inscrições com os dados do aluno e do curso (apenas Admin)
    public async Task<List<EnrollmentDto>> GetAllAsync()
    {
        return await _context.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .Select(e => new EnrollmentDto
            {
                Id           = e.Id,
                CourseId     = e.CourseId,
                StudentName  = e.User.Name,
                StudentEmail = e.User.Email,
                CourseTitle  = e.Course.Title,
                EnrolledAt   = e.CreatedAt
            })
            .ToListAsync();
    }

    // Retorna apenas as inscrições do aluno autenticado (rota /enrollments/mine)
    public async Task<List<EnrollmentDto>> GetByUserIdAsync(int userId)
    {
        return await _context.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .Where(e => e.UserId == userId)
            .Select(e => new EnrollmentDto
            {
                Id           = e.Id,
                CourseId     = e.CourseId,
                StudentName  = e.User.Name,
                StudentEmail = e.User.Email,
                CourseTitle  = e.Course.Title,
                EnrolledAt   = e.CreatedAt
            })
            .ToListAsync();
    }

    // Inscreve um aluno em um curso; retorna o DTO criado ou uma mensagem de erro
    public async Task<(EnrollmentDto? enrollment, string? error)> EnrollAsync(int courseId, int userId)
    {
        // Verifica se o curso existe
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
            return (null, "Curso não encontrado");

        // Impede inscrição duplicada (constraint também existe no banco)
        bool jaInscrito = await _context.Enrollments
            .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

        if (jaInscrito)
            return (null, "Você já está inscrito neste curso");

        var enrollment = new Enrollment
        {
            UserId    = userId,
            CourseId  = courseId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        // Carrega as entidades relacionadas para montar o DTO de resposta
        await _context.Entry(enrollment).Reference(e => e.User).LoadAsync();
        await _context.Entry(enrollment).Reference(e => e.Course).LoadAsync();

        return (new EnrollmentDto
        {
            Id           = enrollment.Id,
            CourseId     = enrollment.CourseId,
            StudentName  = enrollment.User.Name,
            StudentEmail = enrollment.User.Email,
            CourseTitle  = enrollment.Course.Title,
            EnrolledAt   = enrollment.CreatedAt
        }, null);
    }
}
