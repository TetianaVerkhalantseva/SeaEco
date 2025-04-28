using SeaEco.Abstractions.ValueObjects;

namespace SeaEco.Reporter.Models.B2;

public class ColumnB2
{
    public Coordinate Coordinate { get; set; }
    public short Depth { get; set; }
    public byte Attempts { get; set; }
    public bool BubblingWood { get; set; }

    public bool Leire { get; set; }
    public bool Silt { get; set; }
    public bool Sand { get; set; }
    public bool Grus { get; set; }
    public bool Skjellsand { get; set; }
    public bool Steinbunn { get; set; }
    public bool Fjellbunn { get; set; }

    public byte Pigghuder { get; set; }
    public byte Krepsdyr { get; set; }
    public byte Skjell { get; set; }
    public byte Børstemark { get; set; }
}