using Microsoft.EntityFrameworkCore;
using CursosPlatform.api.Models;

namespace CursosPlatform.api.Data;

public class AppDbContext : DbContext
{
    // Construtor que recebe as opções de configuração via injeção de dependência
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Tabela: users ────────────────────────────────────────────────────
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
            entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            entity.Property(u => u.Password).HasColumnName("password").HasMaxLength(255).IsRequired();
            entity.Property(u => u.Role).HasColumnName("role").HasMaxLength(20).IsRequired();
            entity.Property(u => u.CreatedAt).HasColumnName("created_at");

            // E-mail deve ser único no banco
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // ── Tabela: courses ───────────────────────────────────────────────────
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("courses");

            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            entity.Property(c => c.Description).HasColumnName("description");
            entity.Property(c => c.Workload).HasColumnName("workload").IsRequired();
            entity.Property(c => c.CreatedAt).HasColumnName("created_at");
        });

        // ── Tabela: enrollments ───────────────────────────────────────────────
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("enrollments");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            // Índice composto garante que um aluno não se inscreva duas vezes no mesmo curso
            entity.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();

            // Relacionamento: Enrollment → User (muitos para um)
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Enrollments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento: Enrollment → Course (muitos para um)
            entity.HasOne(e => e.Course)
                  .WithMany(c => c.Enrollments)
                  .HasForeignKey(e => e.CourseId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
