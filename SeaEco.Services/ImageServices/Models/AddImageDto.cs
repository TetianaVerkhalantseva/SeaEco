using Microsoft.AspNetCore.Http;

namespace SeaEco.Services.ImageServices.Models;

public sealed class AddImageDto
{
     public IFormFile Image { get; set; }
     public Guid UndersokelseId { get; set; }
     public bool Silt { get; set; }
     public string Posisjon { get; set; } = string.Empty;
}