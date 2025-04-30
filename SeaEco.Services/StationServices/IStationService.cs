using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.StationServices;

public interface IStationService
{
    Task<StationResult> GetStationsByProvetakningsplanIdAsync(Guid samplingPlanId);
    Task<StationResult> GetStationByIdAsync(Guid stationId);
    Task<StationResult> AddStationToPlanAsync(Guid samplingPlanId, NewStationDto dto);
    Task<StationResult> AddStationToProjectAsync(Guid projectId, NewStationDto dto);
    Task<StationResult> UpdateStationAsync(Guid stationId, UpdateStationDto dto);
    Task<StationResult> DeleteStationAsync(Guid stationId);
}