using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.Project;

public class BSensoryDto
{
    public double? ProvePh { get; set; }

    public double? ProveEh { get; set; }

    public double? ProveTemp { get; set; }
    
    public int? Farge { get; set; }

    public int? Lukt { get; set; }

    public int? Konsistens { get; set; }

    public int? Grabbvolum { get; set; }

    public int? Tykkelseslamlag { get; set; }

    public int? Gassbobler { get; set; }
}