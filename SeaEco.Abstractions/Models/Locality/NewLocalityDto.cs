using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.Project;


public class NewLocalityDto
{
    [Required]
    public string Lokalitetsnavn { get; set; } = null!;
    [Required]
    public string LokalitetsId { get; set; } = null!;
    
}
