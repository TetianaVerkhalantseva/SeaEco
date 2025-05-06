namespace SeaEco.Abstractions.Models.Stations;

public class UpdateStationDto
{
    public int NorthDegree { get; }
    public float NorthMinutes { get; }
    public int EastDegree { get; }
    public float EastMinutes { get; }
    public int Dybde { get; set; }
    public string Analyser { get; set; } = string.Empty;
}