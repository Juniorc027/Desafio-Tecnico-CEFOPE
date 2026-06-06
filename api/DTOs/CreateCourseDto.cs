using System.ComponentModel.DataAnnotations;

namespace CursosPlatform.api.DTOs;

// Dados recebidos no corpo da requisição POST /courses
public class CreateCourseDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [MaxLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Carga horária é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Carga horária deve ser maior que zero")]
    public int Workload { get; set; }
}
