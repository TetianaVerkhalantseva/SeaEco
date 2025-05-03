using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;

namespace SeaEco.Services.BSurveyService;


public interface IBSurveyService
{
    Task<SurveyDto?> GetSurveyById(Guid id);
    
    Task<EditSurveyResult> CreateSurvey(AddSurveyDto dto);
}