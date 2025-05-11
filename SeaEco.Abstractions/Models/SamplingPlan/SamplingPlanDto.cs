namespace SeaEco.Abstractions.Models.SamplingPlan;

public class SamplingPlanDto
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public DateOnly? Planlagtfeltdato { get; set; }

    public Guid PlanleggerId { get; set; }
}