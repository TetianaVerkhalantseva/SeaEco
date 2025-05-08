using SeaEco.Abstractions.Enums;

namespace SeaEco.Reporter.Models.Positions;

public class RowPosition
{
    public int Nummer { get; set; }
    
    public string KoordinatNord { get; set; } = null!;

    public string KoordinatOst { get; set; } = null!;

    public int Dybde { get; set; }
    
    public int? AntallGrabbhugg { get; set; }
    
    public Bunntype Bunntype { get; set; }
}