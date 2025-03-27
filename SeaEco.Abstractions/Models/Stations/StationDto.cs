namespace SeaEco.Abstractions.Models.Stations;

public class StationDto
{
    public Guid ProsjektId { get; set; }
    public Guid Stasjonsid { get; set; }
    public DateTime? Datoregistrert { get; set; }
    
    public int Nummer { get; set; }
    public int Dybde { get; set; }
    public int Kordinatern { get; set; }
    public int Kordinatero { get; set; }
    public int SkjovannPh { get; set; }
    public int SkjovannEh { get; set; }
    public int SkjovannTemperatur { get; set; }
    public bool Bunntype { get; set; }
    public bool Dyr { get; set; }
    public int Antallgrabbskudd { get; set; }
    public bool Grabhastighetgodkjent { get; set; }
    public bool? Sensoriskutfort { get; set; }
    public int Bunnsammensettningid { get; set; }
    public string? Arter { get; set; }
    public string? Merknad { get; set; }
    public string? Korrigering { get; set; }
    public int? Grabbid { get; set; }
    public int? Phehmeter { get; set; }
    public DateOnly? Datokalibrert { get; set; }
    public int? Silid { get; set; }
    public string Status { get; set; } = "Ikke registrert";
}