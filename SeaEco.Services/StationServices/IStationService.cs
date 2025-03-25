using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.StationServices;

public interface IStationService
{
    Task<List<StationDto>> GetStationsAsync(Guid prosjektId);
    Task<StationDto?> GetStationByIdAsync(Guid prosjektId, int stasjonsid);
    Task UpdateStationAsync(Guid prosjektId, int stasjonsid, UpdateStationDto dto);
    Task<int> AddExtraStationAsync(Guid prosjektId, NewStationDto dto);
}