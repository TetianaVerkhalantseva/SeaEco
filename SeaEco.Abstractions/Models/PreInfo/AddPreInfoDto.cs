using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.PreInfo;

public class AddPreInfoDto
{
    [Required]
    public Guid ProsjektId { get; set; }
    
    [Required(ErrorMessage = "Feltdato er påkrevd")]
    public DateTime Feltdato { get; set; }
    
    [Required(ErrorMessage = "Du må velge en feltansvarlig")]
    public Guid FeltansvarligId { get; set; }
    
    [Required(ErrorMessage = "Minst en prøvetaker")]
    public List<Guid> ProvetakerIds { get; set; } = new();
    
    [Required(ErrorMessage = "pH er påkrevd")]
    [Range(6, 8.3, ErrorMessage = "pH må være mellom 6 og 8.3")]
    public float Ph { get; set; }
    
    [Required(ErrorMessage = "Eh er påkrevd")]
    [Range(-400, 400, ErrorMessage = "Eh må være mellom -400 og 400 mV")]
    public float Eh { get; set; }
    
    [Required(ErrorMessage = "Temperatur er påkrevd")]
    [Range(-1.8, 25, ErrorMessage = "Temperatur må være mellom -1.8 og 25")]
    public float Temperatur { get; set; }
    
    [Required(ErrorMessage = "Ref.eletrode er påkrevd")]
    public int RefElektrode { get; set; }
    
    [Required(ErrorMessage = "Grabb er påkrevd")]
    public string Grabb { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Sil er påkrevd")]
    public string Sil { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "pH-meter er påkrevd")]
    public string PhMeter { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Datoen er ikke gyldig")]
    public DateOnly Kalibreringsdato { get; set; }
}