namespace CursosPlatform.api.DTOs;

// Dados de uma inscrição retornados pela API (resposta)
public class EnrollmentDto
{
    public int      Id           { get; set; }
    public int      CourseId     { get; set; }
    public string   StudentName  { get; set; } = string.Empty;
    public string   StudentEmail { get; set; } = string.Empty;
    public string   CourseTitle  { get; set; } = string.Empty;
    public DateTime EnrolledAt   { get; set; }
}
