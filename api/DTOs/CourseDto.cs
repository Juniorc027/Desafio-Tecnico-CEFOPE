namespace CursosPlatform.api.DTOs;

// Dados de um curso retornados pela API (resposta)
public class CourseDto
{
    public int    Id              { get; set; }
    public string Title           { get; set; } = string.Empty;
    public string Description     { get; set; } = string.Empty;
    public int    Workload        { get; set; }
    public int    EnrollmentCount { get; set; }
}
