using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.SamplingPlan;

public class EditSamplingPlanDto
{
    [Required(ErrorMessage = "Prosjekt ID er påkrevd")]
    public Guid ProsjektId { get; set; }

    [Required(ErrorMessage = "Planlagt feltdato er påkrevd")]
    public DateOnly Planlagtfeltdato { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Planlegger er påkrevd")]
    public Guid PlanleggerId { get; set; }
}