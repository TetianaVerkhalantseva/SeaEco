namespace SeaEco.Abstractions.Models.Report;

public class GetReportsDto
{
    public IEnumerable<ReportDto> Reports { get; set; } = [];
}