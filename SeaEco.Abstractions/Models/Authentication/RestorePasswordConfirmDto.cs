namespace SeaEco.Abstractions.Models.Authentication;

public class RestorePasswordConfirmDto
{
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}