using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Enums;

namespace SeaEco.Abstractions.Models.Project;

public class UpdateProjectStatusDto
{
    [Required]
    public Prosjektstatus NewStatus { get; set; }

    // Må skrives inn Merknad ved Ferdig/Deaktivert
    public string? Merknad { get; set; }
}