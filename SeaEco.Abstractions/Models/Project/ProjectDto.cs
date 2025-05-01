using SeaEco.Abstractions.Enums;

namespace SeaEco.Abstractions.Models.Project;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string PoId { get; set; } = null!;
    public Guid KundeId { get; set; }
    public string Kundekontaktperson { get; set; } = null!;
    public string Kundetlf { get; set; } = null!;
    public string Kundeepost { get; set; } = null!;
    
    public Guid LokalitetId { get; set; } 
    public string Lokalitetsnavn { get; set; } = null!;
    public string LokalitetsId { get; set; } = null!;
    public int? Mtbtillatelse { get; set; }
    
    public Guid ProsjektansvarligId { get; set; }
    public string? Merknad { get; set; }
    
    public Produksjonsstatus Produksjonsstatus { get; set; }
    public DateTime Datoregistrert { get; set; }
    public int AntallStasjoner { get; set; }
    
}