namespace SeaEco.Abstractions.Models.Project;

public class LocalityDto
{
    public Guid Id { get; set; }
    
    public string Lokalitetsnavn { get; set; } = null!;
    
    public string LokalitetsId { get; set; } = null!;
}