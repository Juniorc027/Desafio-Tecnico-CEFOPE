using CursosPlatform.api.Models;

namespace CursosPlatform.api.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Executa o seed apenas se não houver nenhum usuário cadastrado
        if (context.Users.Any()) return;

        context.Users.AddRange(
            new User
            {
                Name        = "Administrador",
                Email       = "admin@curso.com",
                // HashPassword gera um hash BCrypt seguro com salt automático
                Password    = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role        = "admin",
                CreatedAt   = DateTime.UtcNow
            },
            new User
            {
                Name        = "Aluno Teste",
                Email       = "aluno@curso.com",
                Password    = BCrypt.Net.BCrypt.HashPassword("aluno123"),
                Role        = "student",
                CreatedAt   = DateTime.UtcNow
            }
        );

        // Alguns cursos de exemplo para facilitar os testes
        context.Courses.AddRange(
            new Course
            {
                Title       = "Introdução ao C#",
                Description = "Fundamentos da linguagem C# e .NET para iniciantes.",
                Workload    = 40,
                CreatedAt   = DateTime.UtcNow
            },
            new Course
            {
                Title       = "React do Zero",
                Description = "Criação de interfaces modernas com React e TypeScript.",
                Workload    = 60,
                CreatedAt   = DateTime.UtcNow
            },
            new Course
            {
                Title       = "Banco de Dados com PostgreSQL",
                Description = "Modelagem relacional, consultas SQL e boas práticas.",
                Workload    = 30,
                CreatedAt   = DateTime.UtcNow
            }
        );

        context.SaveChanges();
    }
}
