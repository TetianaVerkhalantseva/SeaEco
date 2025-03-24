using SeaEco.Abstractions.Models.Project;

namespace SeaEco.Services.ProjectServices;

public interface IProjectService
{
    Task<Guid> CreateProjectAsync(NewProjectDto dto);
    Task<List<ProjectDto>> GetProjectsByCustomerAsync(int kundeId);
    Task<ProjectDto?> GetProjectByIdAsync(Guid prosjektId);
}