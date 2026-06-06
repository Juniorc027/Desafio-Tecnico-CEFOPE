namespace CursosPlatform.api.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Armazena o hash BCrypt da senha, nunca a senha em texto puro
    public string Password { get; set; } = string.Empty;

    // Valores possíveis: "admin" | "student"
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Propriedade de navegação: um usuário pode ter várias inscrições
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}