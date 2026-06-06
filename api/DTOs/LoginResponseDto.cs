namespace CursosPlatform.api.DTOs;

// Dados retornados após login bem-sucedido
public class LoginResponseDto
{
    public int    Id    { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Name  { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role  { get; set; } = string.Empty;
}
