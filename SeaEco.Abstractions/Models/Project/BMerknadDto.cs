namespace SeaEco.Abstractions.Models.Project;

public class BMerknadDto
{
    public bool? Detritus { get; set; }
    
    public bool? TerrestriskMateriale { get; set; }
    
    public bool? ResterEtterAnleggsrens { get; set;}

    public string Beskrivelse { get; set; } = "";
}