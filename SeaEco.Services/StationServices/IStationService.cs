using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.StationServices;


public interface IStationService
{
    Task<StationResult> GetStationsByProvetakningsplanIdAsync(Guid samplingPlanId);
    Task<StationResult> GetStationByIdAsync(Guid projectId, Guid stationId);
    Task<StationResult> GetStationsByProjectIdAsync(Guid projectId);
    
    Task<BStationDto?> GetBStationDtoByStationId(Guid projectId, Guid stationId);
    Task<StationResult> AddStationToPlanAsync(Guid samplingPlanId, NewStationDto dto);
    Task<StationResult> AddStationToProjectAsync(Guid projectId, NewStationDto dto);
    Task<StationResult> UpdateStationAsync(Guid stationId, UpdateStationDto dto);
    Task<StationResult> DeleteStationAsync(Guid stationId);
    Task<StationResult> DeleteStationFromProjectAsync(Guid projectId, Guid stationId);
}
