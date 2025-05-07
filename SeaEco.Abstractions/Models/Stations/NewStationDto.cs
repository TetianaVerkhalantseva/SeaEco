namespace SeaEco.Abstractions.Models.Stations;

public class NewStationDto
{
    public Guid ProsjektId { get; set; }
    public int NorthDegree { get; }
    public float NorthMinutes { get; }
    public int EastDegree { get; }
    public float EastMinutes { get; }
    public int Dybde { get; set; }
    public string Analyser { get; set; } = string.Empty;
}
