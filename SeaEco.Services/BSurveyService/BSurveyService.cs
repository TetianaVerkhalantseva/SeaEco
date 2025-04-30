using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.BSurveyService;


public class BSurveyService: IBSurveyService
{
    private readonly AppDbContext _db;

    public BSurveyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BUndersokelse?> GetSurveyById(Guid surveyId)
    {
        var survey = await _db.BUndersokelses
            .Where(s => s.Id == surveyId)
            .FirstOrDefaultAsync();
        
        return survey ?? null;
    }
    
}