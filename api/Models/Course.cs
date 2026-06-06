namespace CursosPlatform.api.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Carga horária em horas
    public int Workload { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Propriedade de navegação: um curso pode ter várias inscrições
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}