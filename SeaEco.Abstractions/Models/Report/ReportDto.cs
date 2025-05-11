namespace SeaEco.Abstractions.Models.Report;

public sealed class ReportDto
{
    public Guid Id { get; set; }
    public string SheetName { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
}