namespace SeaEco.EntityFramework.Entities;

public partial class Kunde
{
    public int Kundeid { get; set; }

    public string Oppdragsgiver { get; set; } = null!;

    public string Kontaktperson { get; set; } = null!;
    
    public string Telefonnummer { get; set; } = null!;

    public virtual ICollection<BProsjekt> BProsjekts { get; set; } = new List<BProsjekt>();
}
