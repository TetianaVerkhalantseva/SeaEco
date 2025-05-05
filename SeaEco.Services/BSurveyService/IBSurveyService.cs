using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.BSurveyService;


public interface IBSurveyService
{
    Task<BUndersokelse?> GetSurveyById(Guid id);

    Task<EditBSurveyResult> CreateSurvey(EditBSurveyDto dto);
    
    
}