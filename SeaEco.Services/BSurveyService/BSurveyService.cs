using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;
using SeaEco.EntityFramework.Contexts;
using SeaEco.Services.Mapping;
using SeaEco.Services.ProjectServices;
using SeaEco.Services.ReportServices;
using SeaEco.Services.StationServices;

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
            .Include(s => s.BStasjon)
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
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            dto.DatoRegistrert = DateTime.SpecifyKind(dto.DatoRegistrert ?? now, DateTimeKind.Unspecified);
            dto.DatoEndret = DateTime.SpecifyKind(dto.DatoEndret ?? now, DateTimeKind.Unspecified);

            dto.Feltdato = DateOnly.FromDateTime(DateTime.Today);

            var searchDate = dto.Feltdato.ToDateTime(TimeOnly.MinValue);

            var preInfo = await _db.BPreinfos
                .FirstOrDefaultAsync(p =>
                    p.ProsjektId == projectId &&
                    p.Feltdato.Date == searchDate.Date);

            if (preInfo != null)
            {
                dto.Feltdato = DateOnly.FromDateTime(DateTime.SpecifyKind(preInfo.Feltdato, DateTimeKind.Unspecified));
                dto.PreinfoId = preInfo.Id;
            }
            else
            {
                return new EditSurveyResult()
                {
                    IsSuccess = false,
                    Message = "Cannot find the preinfo with the same date"
                };
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
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            dto.DatoEndret = DateTime.SpecifyKind(dto.DatoEndret ?? now, DateTimeKind.Unspecified);

            var existing = await _db.BUndersokelses
                .Include(x => x.BStasjon)
                .Include(x => x.Blotbunn)
                .Include(x => x.Hardbunn)
                .Include(x => x.Sediment)
                .Include(x => x.Sensorisk)
                .Include(x => x.Dyr)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (existing == null)
            {
                return new EditSurveyResult
                {
                    IsSuccess = false,
                    Message = "Survey not found."
                };
            }

            existing.ApplyEditSurveyDto(dto);
            await _db.SaveChangesAsync();

            return new EditSurveyResult
            {
                IsSuccess = true,
                Message = "Survey Updated Successfully."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new EditSurveyResult
            {
                IsSuccess = false,
                Message = "An error occurred while updating the survey."
            };
        }
    }

}