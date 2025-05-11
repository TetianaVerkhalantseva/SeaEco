using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.Stations;

public class NewStationDto
{
    public Guid ProsjektId { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageNorthDegree", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int NorthDegree { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageNorthMinutes", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public float NorthMinutes { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageEastDegree", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int EastDegree { get; set; }
    
    [Required(ErrorMessageResourceName = "ErrorMessageEastMinues", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public float EastMinutes { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageDepth", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int Dybde { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageMethod", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Analyser { get; set; } = "Parameter I, II og III";
}
