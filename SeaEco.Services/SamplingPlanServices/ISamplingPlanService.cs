using SeaEco.Abstractions.Models.SamplingPlan;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.SamplingPlanServices;


public interface ISamplingPlanService
{
    Task<SamplingPlanDto?> GetSamplingPlanById(Guid id);
    
    Task<EditSamplingPlanResult> CreateSamplingPlan(EditSamplingPlanDto samplingPlanDto);
    
    Task<EditSamplingPlanResult> UpdateSamplingPlan(Guid id, EditSamplingPlanDto samplingPlanDto);
    
    Task<EditSamplingPlanResult> DeleteSamplingPlan(Guid projectId, Guid id);
}