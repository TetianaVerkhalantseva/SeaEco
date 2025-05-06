namespace SeaEco.Abstractions.Models.PreInfo;

public class PreInfoDto
{
    public Guid Id { get; set; }
    public Guid ProsjektId { get; set; }
    public DateTime Feltdato { get; set; }
    public Guid FeltansvarligId { get; set; }
    public List<Guid> ProvetakerIds { get; set; } = new();
    public float Ph { get; set; }
    public float Eh { get; set; }
    public float Temperatur { get; set; }
    public float? RefElektrode { get; set; } 
    public string Grabb { get; set; } = null!;
    public string Sil { get; set; } = null!;
    public string PhMeter { get; set; } = null!;
    public DateOnly Kalibreringsdato { get; set; }
}
