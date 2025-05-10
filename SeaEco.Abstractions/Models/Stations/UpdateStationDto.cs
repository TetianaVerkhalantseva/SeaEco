namespace SeaEco.Abstractions.Models.Stations;

public class UpdateStationDto
{
    public Guid Id { get; set; }
    public int NorthDegree { get; set; }
    public float NorthMinutes { get; set; }
    public int EastDegree { get; set; }
    public float EastMinutes { get; set; }
    public int Dybde { get; set; }
    public string Analyser { get; set; } = "Parameter I, II og III";
    
    public int? Nummer { get; set; }
    public string? KoordinatNord { get; set; } 
    public string? KoordinatOst { get; set; }
}