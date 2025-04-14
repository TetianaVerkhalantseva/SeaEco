using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.Project;

public class BSensoryDto
{
    public int ProvePh { get; set; }

    public int ProveEh { get; set; }

    public int ProveTemp { get; set; }
    
    public bool Farge { get; set; }

    public int Lukt { get; set; }

    public int Konsistens { get; set; }

    public int Grabbvolum { get; set; }

    public int Tykkelseslamlag { get; set; }

    public int Gassbobler { get; set; }
}