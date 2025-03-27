using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.StationServices;

public interface IStationService
{
    Task<List<StationDto>> GetStationsAsync(Guid prosjektId);
    Task<StationDto?> GetStationByIdAsync(Guid prosjektId, Guid stasjonsid);
    Task UpdateStationAsync(Guid prosjektId, Guid stasjonsid, UpdateStationDto dto);
    Task<int> AddExtraStationAsync(Guid prosjektId, NewStationDto dto);
}