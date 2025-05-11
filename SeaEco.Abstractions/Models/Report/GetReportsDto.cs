namespace SeaEco.Abstractions.Models.Report;

public class GetReportsDto
{
    public ReportDto? Plan { get; set; }
    public IEnumerable<ReportDto> Reports { get; set; } = [];
}