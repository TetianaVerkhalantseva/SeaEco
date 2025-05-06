using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.ProjectServices;

public interface IProjectService
{
    Task<Guid> CreateProjectAsync(NewProjectDto dto);
    Task<ProjectDto?> GetProjectByIdAsync(Guid id);
    Task<List<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto> UpdateProjectAsync(Guid id, EditProjectDto dto);
    Task<string> GenerateAndSetProsjektIdSeAsync(Guid prosjektId, DateTime feltdato);
}