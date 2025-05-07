using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;

namespace SeaEco.Services.BSurveyService;


public interface IBSurveyService
{
    Task<EditSurveyDto?> GetSurveyById(Guid id);
    
    Task<EditSurveyResult> CreateSurvey(Guid projectId, Guid stationId, EditSurveyDto dto);
    
    Task<EditSurveyResult> UpdateSurvey(Guid projectId, Guid stationId, Guid surveyId, EditSurveyDto dto);
}