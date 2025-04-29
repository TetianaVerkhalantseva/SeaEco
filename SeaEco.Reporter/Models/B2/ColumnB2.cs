using SeaEco.Abstractions.ValueObjects;
using SeaEco.Abstractions.ValueObjects.Half;

namespace SeaEco.Reporter.Models.B2;

public class ColumnB2
{
    public Coordinate Coordinate { get; set; }
    public short Depth { get; set; }
    public byte Attempts { get; set; }
    public bool BubblingWood { get; set; }

    public HalfValue Leire { get; set; }
    public HalfValue Silt { get; set; }
    public HalfValue Sand { get; set; }
    public HalfValue Grus { get; set; }
    public HalfValue Skjellsand { get; set; }
    public HalfValue Steinbunn { get; set; }
    public HalfValue Fjellbunn { get; set; }

    public string Pigghuder { get; set; }
    public string Krepsdyr { get; set; }
    public string Skjell { get; set; }
    public string Børstemark { get; set; }

    public bool Beggiota { get; set; }
    public bool Fôr { get; set; }
    public bool Fekalier { get; set; }

    public string Kommentarer { get; set; } = string.Empty;
}