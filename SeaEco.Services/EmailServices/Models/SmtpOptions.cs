namespace SeaEco.Services.EmailServices.Models;

public sealed record SmtpOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}