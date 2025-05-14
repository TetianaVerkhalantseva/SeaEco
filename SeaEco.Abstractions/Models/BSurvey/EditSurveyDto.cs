using SeaEco.Abstractions.Models.Bundersokelse;

namespace SeaEco.Abstractions.Models.BSurvey;

public class EditSurveyDto
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public Guid PreinfoId { get; set; }

    public DateOnly Feltdato { get; set; }

    public int? AntallGrabbhugg { get; set; }

    public bool GrabbhastighetGodkjent { get; set; }

    public Guid? BlotbunnId { get; set; }

    public Guid? HardbunnId { get; set; }

    public Guid? SedimentId { get; set; }

    public Guid? SensoriskId { get; set; }

    public bool Beggiatoa { get; set; }

    public bool Forrester { get; set; }

    public bool Fekalier { get; set; }

    public Guid? DyrId { get; set; }

    public string? Merknader { get; set; }

    public DateTime? DatoRegistrert { get; set; }
    
    public DateTime? DatoEndret { get; set; }

    public float? IndeksGr2Gr3 { get; set; }

    public int? TilstandGr2Gr3 { get; set; }
    
    // dto
    
    public BStationDto? BStation { get; set; }
    
    public BSoftBaseDto? BSoftBase { get; set; }
    
    public BAnimalDto? BAnimal { get; set; }
    
    public BHardBaseDto? BHardBase { get; set; }
    
    public BSedimentDto? BSediment { get; set; }
    
    public BSensoriskDto? BSensorisk { get; set; }
}