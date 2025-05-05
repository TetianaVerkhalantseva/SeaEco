using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.StationServices;

public interface IStationService
{
    Task<List<StationDto>> GetStationsAsync(Guid Id);
    Task<StationDto?> GetStationByIdAsync(Guid Id, Guid stasjonsid);
    Task UpdateStationAsync(Guid Id, Guid stasjonsid, UpdateStationDto dto);
    Task<int> AddExtraStationAsync(Guid Id, NewStationDto dto);
}