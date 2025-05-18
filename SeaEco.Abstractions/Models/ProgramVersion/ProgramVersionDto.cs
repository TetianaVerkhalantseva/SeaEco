namespace SeaEco.Abstractions.Models.ProgramVersion;

public class ProgramVersionDto
{
    public DateOnly Utgivelsesdato { get; set; }

    public string Versjonsnummer { get; set; } = null!;

    public string Forbedringer { get; set; } = null!;
}