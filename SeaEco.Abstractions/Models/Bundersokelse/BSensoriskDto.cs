using SeaEco.Abstractions.Models.Enums;

namespace SeaEco.Abstractions.Models.Bundersokelse;

public class BSensoriskDto
{
    public Gassbobler Gassbobler { get; set; }
    
    public Farge Farge { get; set; }

    public Lukt Lukt { get; set; }

    public Konsistens Konsistens { get; set; }

    public Grabbvolum Grabbvolum { get; set; }

    public Tykkelseslamlag Tykkelseslamlag { get; set; }
}
