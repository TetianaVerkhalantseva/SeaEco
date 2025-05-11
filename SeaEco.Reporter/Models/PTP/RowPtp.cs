namespace SeaEco.Reporter.Models.PTP;

public class RowPtp
{
    public DateOnly Planlagtfeltdato { get; set; }
    
    public int Nummer { get; set; }
    
    public string KoordinatNord { get; set; } = null!;

    public string KoordinatOst { get; set; } = null!;

    public int Dybde { get; set; }
    
    public string Analyser { get; set; } = null!;
}