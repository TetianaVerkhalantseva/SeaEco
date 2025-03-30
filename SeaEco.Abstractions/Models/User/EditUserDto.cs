using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.User;

public sealed class EditUserDto
{
    [Required(ErrorMessage = "Fornavn er påkrevd")]
    [MinLength(3, ErrorMessage = "Fornavn må være minst 3 tegn langt")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Etternavn er påkrevd")]
    [MinLength(3, ErrorMessage = "Etternavn må være minst 3 tegn langt")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email er påkrevd")]
    [EmailAddress(ErrorMessage = "Ugyldig epostadresse")]
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}