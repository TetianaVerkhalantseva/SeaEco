using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Enums.Bsensorisk;

namespace SeaEco.Reporter.Models.B1;

public sealed class ColumnB1
{
    public Bunntype Bunntype { get; set; }
    public Dyr Dyr { get; set; }
    public float pH { get; set; }
    public float Eh { get; set; }
    public int phEh { get; set; }
    public Indeks TilstandGruppeII{ get; set; }

    public Gassbobler Gassbobler { get; set; }
    public Farge Farge { get; set; }
    public Lukt Lukt { get; set; }
    public Konsistens Konsistens { get; set; }
    public Grabbvolum Grabbvolum { get; set; }
    public Tykkelseslamlag Tykkelseslamlag { get; set; }
    public short Sum { get; set; }
    public float KorrigertSum { get; set; }
    public Indeks TilstandGruppeIII { get; set; }

    public float MiddelverdiGruppeIIogIII { get; set; }
    public Indeks TilstandPrøve { get; set; }
}