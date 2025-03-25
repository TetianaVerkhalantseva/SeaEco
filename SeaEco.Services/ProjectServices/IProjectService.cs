using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.Stations;

namespace SeaEco.Services.ProjectServices;

public interface IProjectService
{
    Task<Guid> CreateProjectAsync(NewProjectDto dto);
    Task<ProjectDto?> GetProjectByIdAsync(Guid prosjektId);
    Task<List<ProjectDto>> GetAllProjectsAsync();
    
}