using System.ComponentModel.DataAnnotations;

namespace CursosPlatform.api.DTOs;

// Dados recebidos no corpo da requisição POST /auth/login
public class LoginRequestDto
{
    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Password { get; set; } = string.Empty;
}
