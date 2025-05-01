namespace SeaEco.Abstractions.Models.Stations;

public class StationResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public Guid? StationId { get; set; }
    public StationDto? Station { get; set; }
    public List<StationDto>? Stations { get; set; }
}