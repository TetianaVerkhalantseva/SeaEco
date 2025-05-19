using SeaEco.Abstractions.Models.Lokalitet;

namespace SeaEco.Services.LokalitetServices;

public interface ILokalitetService
{
    Task<List<LokalitetDto>> GetAllAsync();
}
