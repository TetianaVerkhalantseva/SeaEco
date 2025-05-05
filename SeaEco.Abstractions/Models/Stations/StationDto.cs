namespace SeaEco.Abstractions.Models.Stations;

public class StationDto
{
    public Guid Id { get; set; }
    public Guid ProsjektId { get; set; }
    public Guid? ProvetakingsplanId { get; set; }
    public int Nummer { get; set; }
    public string KoordinatNord { get; set; } = null!;
    public string KoordinatOst { get; set; } = null!;
    public int Dybde { get; set; }
    public string Analyser { get; set; } = null!;
}