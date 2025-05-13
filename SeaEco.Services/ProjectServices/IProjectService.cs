using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.ProjectServices;

public interface IProjectService
{
    Task<Guid> CreateProjectAsync(NewProjectDto dto);
    Task<ProjectDto?> GetProjectByIdAsync(Guid id);
    Task<List<ProjectDto>> GetAllProjectsAsync();
    
    Task<List<ProjectDto>> GetAllProjectsByCustomerId(Guid customerId);
    Task<ProjectDto> UpdateProjectAsync(Guid id, EditProjectDto dto);
    Task<string> GenerateAndSetProsjektIdSeAsync(Guid prosjektId, DateTime feltdato);
    Task UpdateProjectStatusAsync(Guid projectId, Prosjektstatus newStatus, string? merknad = null);
    Task AddMerknadAsync(Guid projectId, string merknad);
}