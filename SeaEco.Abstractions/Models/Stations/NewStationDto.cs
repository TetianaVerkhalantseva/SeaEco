namespace SeaEco.Abstractions.Models.Stations;

public class NewStationDto
{
    public Guid ProsjektId { get; set; }
    public int NorthDegree { get; set; }
    public float NorthMinutes { get; set; }
    public int EastDegree { get; set; }
    public float EastMinutes { get; set; }
    public int Dybde { get; set; }
    public string Analyser { get; set; } = "Parameter I, II og III";
}
