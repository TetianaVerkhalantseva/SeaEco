namespace SeaEco.Abstractions.Models.Stations;

public class UpdateStationDto
{
    public string KoordinatNord { get; set; } = string.Empty;
    public string KoordinatOst { get; set; } = string.Empty;
    public int Dybde { get; set; }
    public string Analyser { get; set; } = string.Empty;
}