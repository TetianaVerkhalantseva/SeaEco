namespace SeaEco.Abstractions.Models.BSurvey;

public class BPreInfoDto
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public DateTime Feltdato { get; set; }

    public Guid FeltansvarligId { get; set; }
}