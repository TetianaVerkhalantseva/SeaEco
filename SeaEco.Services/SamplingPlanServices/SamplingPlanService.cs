using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.SamplingPlan;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.SamplingPlanServices;


public class SamplingPlanService: ISamplingPlanService
{
    private readonly AppDbContext _db;

    public SamplingPlanService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BProvetakningsplan?> GetSamplingPlanById(Guid id)
    {
        var samplingPlan = await _db.BProvetakningsplans
            .Where(s => s.Id == id).FirstOrDefaultAsync();

        return samplingPlan ?? null;
    }

    public async Task<EditSamplingPlanResult> CreateSamplingPlan(EditSamplingPlanDto samplingPlanDto)
    {
        var projectExists = await _db.BProsjekts.AnyAsync(p => p.Id == samplingPlanDto.ProsjektId);
        if (!projectExists)
        {
            return new EditSamplingPlanResult
            {
                IsSuccess = false,
                Message = $"Project with ID {samplingPlanDto.ProsjektId} does not exist."
            };
        }
        
        var samplingPlanExists = await _db.BProvetakningsplans.AnyAsync(p => p.ProsjektId == samplingPlanDto.ProsjektId);
        if (samplingPlanExists)
        {
            return new EditSamplingPlanResult
            {
                IsSuccess = false,
                Message = "A sampling plan already exists for this project."
            };
        }
        
        var newPlan = new BProvetakningsplan()
        {
            Id = Guid.NewGuid(),
            ProsjektId = samplingPlanDto.ProsjektId,
            Planlagtfeltdato = samplingPlanDto.Planlagtfeltdato,
            PlanleggerId = samplingPlanDto.PlanleggerId
        };

        try
        {
            await _db.BProvetakningsplans.AddAsync(newPlan);
            await _db.SaveChangesAsync();
            return new EditSamplingPlanResult()
            {
                IsSuccess = true,
                Message = $"Sampling plan created successfully with planning date: {newPlan.Planlagtfeltdato}"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = "An error occured while creating sampling plan."
            };
        }
    }

    public async Task<EditSamplingPlanResult> UpdateSamplingPlan(Guid id, EditSamplingPlanDto samplingPlanDto)
    {
        var samplingPlan = await _db.BProvetakningsplans.FirstOrDefaultAsync(s => s.Id == id);
        if (samplingPlan == null)
        {
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = $"Sampling plan with id {id} not found!",
            };
        }

        if (samplingPlan.ProsjektId != samplingPlanDto.ProsjektId)
        {
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = "Mismatch between sampling plan and project ID."
            };
        }
        
        samplingPlan.Planlagtfeltdato = samplingPlanDto.Planlagtfeltdato;
        samplingPlan.PlanleggerId = samplingPlanDto.PlanleggerId;

        try
        {
            await _db.SaveChangesAsync();
            return new EditSamplingPlanResult()
            {
                IsSuccess = true,
                Message = $"Sampling plan edited successfully with planning date: {samplingPlan.Planlagtfeltdato}"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = "Error occurred while updating sampling plan."
            };
        }
    }

    public async Task<EditSamplingPlanResult> DeleteSamplingPlan(Guid projectId, Guid id)
    {
        var projectExists = await _db.BProsjekts.AnyAsync(p => p.Id == projectId);
        if (!projectExists)
        {
            return new EditSamplingPlanResult
            {
                IsSuccess = false,
                Message = $"Project with ID {projectId} does not exist."
            };
        }
        
        var samplingPlan = await _db.BProvetakningsplans
            .FirstOrDefaultAsync(s => s.Id == id && s.ProsjektId == projectId);
        if (samplingPlan == null)
        {
            return new EditSamplingPlanResult
            {
                IsSuccess = false,
                Message = $"Sampling plan with ID {id} not found in project {projectId}."
            };
        }

        try
        {
            _db.BProvetakningsplans.Remove(samplingPlan);
            await _db.SaveChangesAsync();
            return new EditSamplingPlanResult()
            {
                IsSuccess = true,
                Message = "Sampling plan deleted successfully!"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = "Error occurred while deleting sampling plan."
            };
        }

    }
}