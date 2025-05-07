namespace SeaEco.Abstractions.Models.BSurvey;

public class BStationDto
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public int Nummer { get; set; }

    public string KoordinatNord { get; set; } = null!;

    public string KoordinatOst { get; set; } = null!;

    public int Dybde { get; set; }

    public string Analyser { get; set; } = null!;

    public Guid? ProvetakingsplanId { get; set; }

    public Guid? UndersokelseId { get; set; }
}