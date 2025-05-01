using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.BSurveyService;


public interface IBSurveyService
{
    Task<SurveyDto?> GetSurveyById(Guid id);
    
}