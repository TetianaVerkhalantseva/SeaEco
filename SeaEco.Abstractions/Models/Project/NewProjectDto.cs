namespace SeaEco.Abstractions.Models.Project;

public class NewProjectDto
{
    public string PoId { get; set; } = null!;
    public int KundeId { get; set; }
    public string Kundekontaktpersons { get; set; } = null!;
    public int Kundetlf { get; set; }
    public string Kundeepost { get; set; } = null!;
    
    public string Lokalitet { get; set; } = null!;
    public int Lokalitetid { get; set; }
    public int Mtbtillatelse { get; set; }
    public int Biomasse { get; set; }
    public DateOnly Planlagtfeltdato { get; set; }
    
    public Guid Ansvarligansattid { get; set; }
    public Guid? Ansvarligansatt2id { get; set; }
    public Guid? Ansvarligansatt3id { get; set; }
    public Guid? Ansvarligansatt4id { get; set; }
    public Guid? Ansvarligansatt5id { get; set; }
    public int Antallstasjoner { get; set; }
    public string? Merknad { get; set; }
    public string Status { get; set; } = "Nytt";
}