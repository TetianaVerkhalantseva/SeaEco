using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Enums;

namespace SeaEco.Abstractions.Models.Project;

public class MerknadDto
{
    [Required]
    public string Merknad { get; set; } = null!;
}

public class UpdateStatusDto
{
    [Required]
    public Prosjektstatus Status { get; set; }
    public string? Merknad { get; set; }
}