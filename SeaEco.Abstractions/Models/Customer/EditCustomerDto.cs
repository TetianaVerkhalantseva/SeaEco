namespace SeaEco.Abstractions.Models.Customer;
using System.ComponentModel.DataAnnotations;

public class EditCustomerDto
{
    [Required(ErrorMessage = "Oppdragsgiver er påkrevd")]
    [MinLength(3, ErrorMessage = "Oppdragsgiver må være minst 3 tegn langt")]
    public string Oppdragsgiver { get; set; } = null!;

    
    [Required(ErrorMessage = "Kontaktperson er påkrevd")]
    [MinLength(3, ErrorMessage = "Kontaktperson må være minst 3 tegn langt")]
    public string Kontaktperson { get; set; } = null!;
    
    
    [Required(ErrorMessage = "Telefonnummer er påkrevd")]
    [MinLength(8, ErrorMessage = "Telefonnummeret må være minst 8 sifre")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "Telefonnummeret må kun inneholde tall")]
    public string Telefonnummer { get; set; } = null!;
}