using SeaEco.Abstractions.Models.ProgramVersion;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.ProgramVersionServices;

public interface IProgramVersionService
{
    Task<Response<ProgramVersionDto>> CurrentVersion();
    Task<Response<ProgramVersionDto>> GetVersion(string version);
    Task<IEnumerable<ProgramVersionDto>> GetVersions();
}
