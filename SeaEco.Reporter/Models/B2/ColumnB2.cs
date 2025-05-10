using SeaEco.Abstractions.ValueObjects;
using SeaEco.Abstractions.ValueObjects.Bunnsubstrat;

namespace SeaEco.Reporter.Models.B2;

public class ColumnB2
{
    public int Nummer { get; set; }
    public string KoordinatNord { get; set; }
    public string KoordinatOst { get; set; }
    public int Dyp { get; set; }
    public int AntallGrabbhugg { get; set; }
    public bool Bobling { get; set; }

    public BunnsubstratValue Leire { get; set; }
    public BunnsubstratValue Silt { get; set; }
    public BunnsubstratValue Sand { get; set; }
    public BunnsubstratValue Grus { get; set; }
    public BunnsubstratValue Skjellsand { get; set; }
    public BunnsubstratValue Steinbunn { get; set; }
    public BunnsubstratValue Fjellbunn { get; set; }

    public string Pigghuder { get; set; }
    public string Krepsdyr { get; set; }
    public string Skjell { get; set; }
    public string Børstemark { get; set; }

    public bool Beggiota { get; set; }
    public bool Fôr { get; set; }
    public bool Fekalier { get; set; }

    public string Kommentarer { get; set; } = string.Empty;
}