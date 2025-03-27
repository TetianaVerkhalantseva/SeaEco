namespace SeaEco.Abstractions.Models.Customer;

public class EditCustomerDto
{
    public string Oppdragsgiver { get; set; } = null!;

    public string Kontaktperson { get; set; } = null!;
    
    public string Telefonnummer { get; set; } = null!;
}