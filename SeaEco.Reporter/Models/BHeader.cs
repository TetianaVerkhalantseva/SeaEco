namespace SeaEco.Reporter.Models;

public sealed class BHeader
{
    public string Oppdragsgiver { get; set; } = string.Empty;
    public IEnumerable<DateTime> FeltDatoer { get; set; } = [];
    public string Lokalitetsnavn { get; set; } = string.Empty;
    public string LokalitetsID { get; set; } = string.Empty;
}