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
        catch (Exception)
        {
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = "Something went wrong!",
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
        
        samplingPlan.ProsjektId = samplingPlanDto.ProsjektId;
        samplingPlan.Planlagtfeltdato = samplingPlanDto.Planlagtfeltdato;
        samplingPlan.PlanleggerId = samplingPlanDto.PlanleggerId;

        try
        {
            _db.BProvetakningsplans.Update(samplingPlan);
            await _db.SaveChangesAsync();
            return new EditSamplingPlanResult()
            {
                IsSuccess = true,
                Message = $"Sampling plan edited successfully with planning date: {samplingPlan.Planlagtfeltdato}"
            };
        }
        catch (Exception)
        {
            return new EditSamplingPlanResult()
            {
                IsSuccess = false,
                Message = "Something went wrong!"
            };
        }
    }

    public async Task<EditSamplingPlanResult> DeleteSamplingPlan(Guid id)
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
        
        _db.BProvetakningsplans.Remove(samplingPlan);
        await _db.SaveChangesAsync();
        return new EditSamplingPlanResult()
        {
            IsSuccess = true,
            Message = "Sampling plan deleted successfully!"
        };
    }
}