namespace SeaEco.Abstractions.Models.Authentication;

public class ResetPasswordConfirmDto
{
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}