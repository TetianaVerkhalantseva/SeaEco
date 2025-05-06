using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.PreInfo;

public class AddPreInfoDto
{
    public Guid ProsjektId { get; set; }
    public DateTime Feltdato { get; set; }
    public Guid FeltansvarligId { get; set; }
    public List<Guid> ProvetakerIds { get; set; } = new();
    public float Ph { get; set; }
    public float Eh { get; set; }
    public float Temperatur { get; set; }
    public int? RefElektrode { get; set; }
    public string Grabb { get; set; } = string.Empty;
    public string Sil { get; set; } = string.Empty;
    public string PhMeter { get; set; } = string.Empty;
    public DateOnly Kalibreringsdato { get; set; }
}