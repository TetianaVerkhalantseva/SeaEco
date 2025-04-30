using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Models.BSurvey;

public class EditBSurveyDto
{
    [Required(ErrorMessage = "Prosjekt ID er påkrevd")]
    public Guid ProsjektId { get; set; }

    [Required(ErrorMessage = "Preinfo ID er påkrevd")]
    public Guid PreinfoId { get; set; }

    [Required(ErrorMessage = "Felt dato er påkrevd")]
    public DateOnly Feltdato { get; set; }
    
    public int? AntallGrabbhugg { get; set; }

    [Required(ErrorMessage = "Godkjent av grabbhastighet er påkrevd")]
    public bool GrabbhastighetGodkjent { get; set; }

    public Guid? BlotbunnId { get; set; }

    public Guid? HardbunnId { get; set; }

    public Guid? SedimentId { get; set; }

    public Guid? SensoriskId { get; set; }

    [Required(ErrorMessage = "Beggiatoa er påkrevd")]
    public bool Beggiatoa { get; set; }

    [Required(ErrorMessage = "Forrester er påkrevd")]
    public bool Forrester { get; set; }

    [Required(ErrorMessage = "Fekalier er påkrevd")]
    public bool Fekalier { get; set; }

    public Guid? DyrId { get; set; }

    public string? Merknader { get; set; }

    public DateTime? DatoRegistrert { get; set; }

    public DateTime? DatoEndret { get; set; }

    public float? IndeksGr2Gr3 { get; set; }

    public int? TilstandGr2Gr3 { get; set; }
}