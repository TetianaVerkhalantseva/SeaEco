using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.Services.Mapping;

namespace SeaEco.Services.BSurveyService;


public class BSurveyService: IBSurveyService
{
    private readonly AppDbContext _db;

    public BSurveyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SurveyDto?> GetSurveyById(Guid id)
    {
        var survey = await _db.BUndersokelses
            .Include(s => s.Preinfo)
            .Include(s => s.BBilders)
            .Include(s => s.BStasjon)
            .Include(s => s.BUndersokelsesloggs)
            .Include(s => s.Blotbunn)
            .Include(s => s.Hardbunn)
            .Include(s => s.Sediment)
            .Include(s => s.Sensorisk)
            .Include(s => s.Dyr)
            .SingleOrDefaultAsync(s => s.Id == id);

        return survey?.ToSurveyDto();
    }

}