using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.Project;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string PoId { get; set; } = null!;
    public Guid KundeId { get; set; }
    public string Oppdragsgiver        { get; set; } = null!;
    [Required(ErrorMessageResourceName = "ErrorMessageCustomer", ErrorMessageResourceType = typeof(ResourcesAbstractions))]

    public string Kundekontaktperson { get; set; } = null!;
    public string Kundetlf { get; set; } = null!;
    [Required(ErrorMessageResourceName = "ErrorMessageMailRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [EmailAddress(ErrorMessageResourceName = "ErrorMessageMailError", ErrorMessageResourceType = typeof(ResourcesAbstractions))]

    public string Kundeepost { get; set; } = null!;
    
    public Guid LokalitetId { get; set; } 
    [Required(ErrorMessageResourceName = "ErrorMessageLocalityName", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Lokalitetsnavn { get; set; } = null!;
    [Required(ErrorMessageResourceName = "ErrorMessageLocalityId", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [MinLength(5, ErrorMessageResourceName = "ErrorMessageLocalityIdLength", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [MaxLength(5, ErrorMessageResourceName = "ErrorMessageLocalityIdLength", ErrorMessageResourceType = typeof(ResourcesAbstractions))] 
    [RegularExpression(@"^\d+$", ErrorMessageResourceName = "ErrorMessageLocalityIdNumber", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string LokalitetsId { get; set; } = null!;
    [Required(ErrorMessageResourceName = "ErrorMessageProjectMtb", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int Mtbtillatelse { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageProjectResponsible", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public Guid? ProsjektansvarligId { get; set; }
    public string? Merknad { get; set; }
    
    public Produksjonsstatus Produksjonsstatus { get; set; }

    public int AntallStasjoner { get; set; } 
    
    public Prosjektstatus Prosjektstatus { get; set; }
    public Tilstand? Tilstand { get; set; }
    public string? ProsjektIdSe { get; set; }
    public List<DateTime> Feltdatoer { get; set; } = new();
}