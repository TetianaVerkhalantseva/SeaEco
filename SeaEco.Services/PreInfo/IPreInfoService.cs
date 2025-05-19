using SeaEco.Abstractions.Models.PreInfo;

namespace SeaEco.Services.PreInfo;

public interface IPreInfoService
{
    Task<List<PreInfoDto>> GetAllByProjectAsync(Guid prosjektId);
    Task<PreInfoDto?> GetByIdAsync(Guid preInfoId);
    Task<Guid> CreatePreInfoAsync(AddPreInfoDto dto);
    Task<PreInfoDto?> EditPreInfoAsync(Guid preInfoId, EditPreInfoDto dto);
    Task<bool> DeletePreInfoAsync(Guid preInfoId);
}
