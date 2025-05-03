namespace SeaEco.Abstractions.Models.Lokalitet;

public class LokalitetDto
{
    public Guid Id { get; set; }
    public string Navn { get; set; } = null!;
    public string LokalitetKode { get; set; } = null!;
}