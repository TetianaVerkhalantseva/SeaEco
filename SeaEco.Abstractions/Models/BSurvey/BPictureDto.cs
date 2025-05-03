namespace SeaEco.Abstractions.Models.BSurvey;

public class BPictureDto
{
    public Guid Id { get; set; }

    public Guid UndersokelseId { get; set; }

    public bool Silt { get; set; }

    public string Extension { get; set; } = null!;
}