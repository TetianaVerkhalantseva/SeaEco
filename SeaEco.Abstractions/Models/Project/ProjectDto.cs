namespace SeaEco.Abstractions.Models.Project;

public class ProjectDto
{
    public Guid ProsjektId { get; set; }
    public string PoId { get; set; } = null!;
    
    public int KundeId { get; set; }
    public string Kundekontaktpersons { get; set; } = null!;
    public string Kundetlf { get; set; }
    public string Kundeepost { get; set; } = null!;
    
    public int Lokalitetid { get; set; }
    public string Lokalitet { get; set; } = null!;
    
    public int Antallstasjoner { get; set; }
    public int Mtbtillatelse { get; set; }
    public int Biomasse { get; set; }
    public DateOnly Planlagtfeltdato { get; set; }
    public string? Merknad { get; set; }
    public string Status { get; set; } = null!;
    public DateTime? Datoregistrert { get; set; }
}