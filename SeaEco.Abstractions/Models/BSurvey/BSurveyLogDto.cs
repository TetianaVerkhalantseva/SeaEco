namespace SeaEco.Abstractions.Models.BSurvey;

public class BSurveyLogDto
{
    public Guid Id { get; set; }

    public string Felt { get; set; } = null!;

    public string GammelVerdi { get; set; } = null!;

    public string NyVerdi { get; set; } = null!;

    public DateTime DatoEndret { get; set; }

    public Guid EndretAv { get; set; }

    public Guid UndersokelseId { get; set; }
}