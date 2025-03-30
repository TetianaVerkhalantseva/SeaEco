using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.Authentication;

public sealed class RegisterUserDto
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

    [Required(ErrorMessage = "Passord er påkrevd")]
    [MinLength(8, ErrorMessage = "Passordet må være minst 8 tegn langt")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).+$", ErrorMessage = "Passordet må inneholde minst en stor bokstav og ett spesialtegn")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Gjenta passord er påkrevd")]
    [Compare("Password", ErrorMessage = "Passordene stemmer ikke overens")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}