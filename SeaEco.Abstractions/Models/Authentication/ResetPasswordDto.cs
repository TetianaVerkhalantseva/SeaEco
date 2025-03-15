namespace SeaEco.Abstractions.Models.Authentication;

public sealed class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
}