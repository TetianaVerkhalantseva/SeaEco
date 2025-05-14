using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Enums.Bsensorisk;

namespace SeaEco.Reporter.Models.B1;

public sealed class ColumnB1
{
    public int Nummer { get; set; }
    
    public Bunntype Bunntype { get; set; }
    public Dyr Dyr { get; set; }

    public bool HasSediment { get; set; }
    public float pH { get; set; }
    public float Eh { get; set; }
    public int phEh { get; set; }
    public Tilstand TilstandProveGr2 { get; set; }

    public bool HasSensorisk { get; set; }
    public Gassbobler Gassbobler { get; set; }
    public Farge Farge { get; set; }
    public Lukt Lukt { get; set; }
    public Konsistens Konsistens { get; set; }
    public Grabbvolum Grabbvolum { get; set; }
    public Tykkelseslamlag Tykkelseslamlag { get; set; }
    public int Sum { get; set; }
    public float KorrigertSum { get; set; }
    public Tilstand TilstandProveGr3 { get; set; }


    public float MiddelVerdiGr2Gr3 { get; set; }
    public Tilstand TilstandProveGr2Gr3 { get; set; }
}