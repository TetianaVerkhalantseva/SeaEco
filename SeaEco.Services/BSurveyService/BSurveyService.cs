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

    public async Task<EditSurveyResult> CreateSurvey(Guid projectId, Guid stationId, EditSurveyDto dto)
    {
        try
        {
            var targetDate = dto.Feltdato.ToDateTime(TimeOnly.MinValue);
            var nextDay = targetDate.AddDays(1);

            var preInfo = await _db.BPreinfos
                .FirstOrDefaultAsync(p =>
                    p.ProsjektId == projectId &&
                    p.Feltdato >= targetDate &&
                    p.Feltdato < nextDay);

            if (preInfo != null)
            {
                dto.PreinfoId = preInfo.Id;
            }
            else
            {
                var newPreInfo = await _db.BPreinfos
                    .FirstOrDefaultAsync(p => p.ProsjektId == projectId);
                if (newPreInfo != null)
                {
                    dto.PreinfoId = newPreInfo.Id;
                }
            }
            
            dto.Id = Guid.NewGuid();
            dto.ProsjektId = projectId;
            dto.BlotbunnId = Guid.NewGuid();
            dto.HardbunnId = Guid.NewGuid();
            dto.SedimentId = Guid.NewGuid();
            dto.SensoriskId = Guid.NewGuid();
            dto.DyrId = Guid.NewGuid();
            dto.DatoRegistrert ??= DateTime.Now;
            
            if (dto.BStation != null)
            {
                dto.ProsjektId = projectId;
                dto.BStation.Id = stationId;
            }
            
            if (dto.BSoftBase != null)
            {
                dto.BSoftBase.Id = Guid.NewGuid();
            }

            if (dto.BAnimal != null)
            {
                dto.BAnimal.Id = Guid.NewGuid();
            }

            if (dto.BHardBase != null)
            {
                dto.BHardBase.Id = Guid.NewGuid();
            }

            if (dto.BSediment != null)
            {
                dto.BSediment.Id = Guid.NewGuid();
            }

            if (dto.BSensorisk != null)
            {
                dto.BSensorisk.Id = Guid.NewGuid();
            }

            foreach (var pic in dto.BBilders)
            {
                pic.Id = Guid.NewGuid();
                pic.UndersokelseId = dto.Id;
                // Silt and Extension attributes are missing here
            }

            foreach (var log in dto.BSurveyLogs)
            {
                log.Id = Guid.NewGuid();
                // How do I record user changes?
            }
            
            var entity = dto.ToEntity();
            _db.BUndersokelses.Add(entity);
            await _db.SaveChangesAsync();
            
            return new EditSurveyResult
            {
                IsSuccess = true,
                Message = "Survey Created Successfully."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new EditSurveyResult
            {
                IsSuccess = false,
                Message = "An error occured while creating the survey."
            };
        }
    }

    public async Task<EditSurveyResult> UpdateSurvey(Guid projectId, Guid stationId, Guid surveyId, EditSurveyDto dto)
    {
        try
        {
            // TODO: update survey
            return new EditSurveyResult
            {
                IsSuccess = true,
                Message = "Survey Updated Successfully."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new EditSurveyResult
            {
                IsSuccess = false,
                Message = "An error occured while updating the survey."
            };
        }
    }
}