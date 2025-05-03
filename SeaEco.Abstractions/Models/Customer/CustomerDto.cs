namespace SeaEco.Abstractions.Models.Customer;

public class CustomerDto
{
    public Guid Id { get; set; }

    public string Oppdragsgiver { get; set; } = null!;

    public string Kontaktperson { get; set; } = null!;

    public string Telefon { get; set; } = null!;
}