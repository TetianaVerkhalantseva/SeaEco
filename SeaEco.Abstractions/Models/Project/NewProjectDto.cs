using SeaEco.Abstractions.Enums;

namespace SeaEco.Abstractions.Models.Project;

public class NewProjectDto
{
    public string PoId { get; set; } = null!;
    public Guid KundeId { get; set; }
    public string Kundekontaktperson { get; set; } = null!;
    public string Kundetlf { get; set; }
    public string Kundeepost { get; set; } = null!;
    
    public Guid Lokalitetid { get; set; } //Se hvordan lokalitet endret. Dette er Fk til Lokalitet tabell - der lokalitetsnavn oh lokalitetsID
    public int Mtbtillatelse { get; set; }
    
    public Guid ProsjektansvarligId { get; set; }
    public string? Merknad { get; set; }
    public Produksjonsstatus Produksjonsstatus { get; set; }
    public DateTime Datoregistrert { get; set; }
}