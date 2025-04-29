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

    public async Task<EditBSurveyResult> CreateSurvey(EditBSurveyDto dto)
    {
        var newSurvey = new BUndersokelse()
        {
            Id = Guid.NewGuid(),
            ProsjektId = dto.ProsjektId,
            PreinfoId = dto.PreinfoId,
            Feltdato = dto.Feltdato,
            AntallGrabbhugg = dto.AntallGrabbhugg,
            GrabbhastighetGodkjent = dto.GrabbhastighetGodkjent,
            BlotbunnId = dto.BlotbunnId,
            HardbunnId = dto.HardbunnId,
            SedimentId = dto.SedimentId,
            SensoriskId = dto.SensoriskId,
            Beggiatoa = dto.Beggiatoa,
            Forrester = dto.Forrester,
            Fekalier = dto.Fekalier,
            DyrId = dto.DyrId,
            Merknader = dto.Merknader,
            DatoRegistrert = dto.DatoRegistrert,
            DatoEndret = dto.DatoEndret,
            IndeksGr2Gr3 = dto.IndeksGr2Gr3,
            TilstandGr2Gr3 = dto.TilstandGr2Gr3
        };

        try
        {
            await _db.BUndersokelses.AddAsync(newSurvey);
            await _db.SaveChangesAsync();
            return new EditBSurveyResult()
            {
                IsSuccess = true,
                Message = "Survey Created Successfully."
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new EditBSurveyResult()
            {
                IsSuccess = false,
                Message = "Something went wrong!"
            };
        }
    }
}