namespace SeaEco.Abstractions.Models.Customer;

public class EditCustomerDto
{
    public string Oppdragsgiver { get; set; } = null!;

    public string Kontaktperson { get; set; } = null!;
    
    public string Telefonnummer { get; set; } = null!;

    public int Orgnr { get; set; }

    public string Postadresse { get; set; } = null!;

    public string Kommune { get; set; } = null!;

    public string Fylke { get; set; } = null!;
}