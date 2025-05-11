using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;
using SeaEco.EntityFramework.Contexts;
using SeaEco.Services.Mapping;
using SeaEco.Services.ProjectServices;
using SeaEco.Services.ReportServices;

namespace SeaEco.Services.BSurveyService;


public class BSurveyService: IBSurveyService
{
    private readonly AppDbContext _db;
    private readonly IProjectService _projectService;
    private readonly IReportService  _reportService;

    public BSurveyService(
        AppDbContext db,
        IProjectService projectService,
        IReportService reportService)
    {
        _db = db;
        _projectService = projectService;
        _reportService  = reportService;
    }

    public async Task<EditSurveyDto?> GetSurveyById(Guid id)
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

        return survey == null ? null : survey.ToSurveyDto();
    }

    public async Task<EditSurveyResult> CreateSurvey(Guid projectId, Guid stationId, EditSurveyDto dto)
    {
        try
        {
            dto.Id = Guid.NewGuid();
            dto.ProsjektId = projectId;
            
            var targetDate = DateTime.Today;

            var preInfo = await _db.BPreinfos
                .FirstOrDefaultAsync(p =>
                    p.ProsjektId == projectId &&
                    p.Feltdato.Date == targetDate);
            
            if (preInfo != null)
            {
                dto.Feltdato = DateOnly.FromDateTime(preInfo.Feltdato);
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
            
            dto.DatoRegistrert ??= DateTime.Now;
            dto.DatoEndret ??= DateTime.Now;

            // foreach (var log in dto.BSurveyLogs)
            // {
            //     log.Id = Guid.NewGuid();
            // }
            
            var entity = dto.ToEntity();
            _db.BUndersokelses.Add(entity);
            await _db.SaveChangesAsync();
            
            var proj = await _projectService.GetProjectByIdAsync(projectId);
            if (proj?.Prosjektstatus == Prosjektstatus.Pabegynt)
            {
                await _projectService.UpdateProjectStatusAsync(
                    projectId,
                    Prosjektstatus.Pagar,
                    merknad: null
                );
                
                var rpt = await _reportService.GeneratePtpReport(projectId);
                if (rpt.IsError)
                {
                    Console.Error.WriteLine($"PTP‐generering feilet for prosjekt {projectId}: {rpt.ErrorMessage}");
                }
            }
            
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
            dto.DatoEndret ??= DateTime.Now;
            
            // TODO: How do I record user changes?
            // foreach (var log in dto.BSurveyLogs)
            // {
            // }
            
            var entity = dto.ToEntity();
            _db.BUndersokelses.Update(entity);
            await _db.SaveChangesAsync();
            
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