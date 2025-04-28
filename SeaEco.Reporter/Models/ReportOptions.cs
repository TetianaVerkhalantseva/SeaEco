namespace SeaEco.Reporter.Models;

public sealed class ReportOptions
{
    public string TemplatePath { get; set; } = string.Empty;
    public string DestinationPath { get; set; } = string.Empty;
    public string NonCommercialPersonalName { get; set; } = string.Empty;
}