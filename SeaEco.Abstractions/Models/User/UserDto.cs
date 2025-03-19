namespace SeaEco.Abstractions.Models.User;

public sealed class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
    public DateTime Datoregistrert { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";    
}