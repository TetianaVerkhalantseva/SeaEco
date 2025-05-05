using SeaEco.Abstractions.Enums.Bsensorisk;

namespace SeaEco.Abstractions.Models.Bundersokelse;

public class BSensoriskDto
{
    public Guid Id { get; set; }

    public int Gassbobler { get; set; }

    public int Farge { get; set; }

    public int Lukt { get; set; }

    public int Konsistens { get; set; }

    public int Grabbvolum { get; set; }

    public int Tykkelseslamlag { get; set; }

    public float? IndeksGr3 { get; set; }

    public int? TilstandGr3 { get; set; }
}
