namespace SeaEco.Reporter.Models.Headers;

public class PtpHeader
{
    public string Lokalitetsnavn { get; set; } = String.Empty;
    
    public string Oppdragsgiver { get; set; } = String.Empty;
    
    public DateOnly Planlagtfeltdato { get; set; }
    
    public string Planlegger { get; set; } = String.Empty;
}