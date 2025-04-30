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
            .Include(s=> s. Prosjekt)
            .Include(s => s.Preinfo)
            .Include(s => s.BBilders)
            .Include(s => s.BUndersokelsesloggs)
            .Include(s => s.Blotbunn)
            .Include(s => s.Hardbunn)
            .Include(s => s.Sediment)
            .Include(s => s.Sensorisk)
            .Include(s => s.Dyr)
            .Include(s => s.BStasjon)
            .FirstOrDefaultAsync(s => s.Id == surveyId);
        
        return survey ?? null;
    }
    
}