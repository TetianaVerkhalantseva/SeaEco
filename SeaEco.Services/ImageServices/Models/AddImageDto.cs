using Microsoft.AspNetCore.Http;

namespace SeaEco.Services.ImageServices.Models;

public sealed class AddImageDto
{
     public IFormFile Image { get; set; }
     public bool Silt { get; set; }
     public string Posisjon { get; set; } = string.Empty;
     public Guid Prosjektid { get; set; }
     public Guid Stasjonsid { get; set; }
}