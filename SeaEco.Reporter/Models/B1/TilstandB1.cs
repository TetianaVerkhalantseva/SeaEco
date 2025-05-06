using SeaEco.Abstractions.Enums;

namespace SeaEco.Reporter.Models.B1;

public class TilstandB1
{
    public float IndeksGr2 { get; set; }
    public Tilstand TilstandGr2 { get; set; }


    public float IndeksGr3 { get; set; }
    public Tilstand TilstandGr3 { get; set; }


    public float LokalitetsIndeks { get; set; }
    public Tilstand LokalitetsTilstand { get; set; }
}