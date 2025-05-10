using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.ReportServices;

public interface IReportService
{
    Task<Response<string>> GenerateInfoReport(Guid projectId);
    Task<Response<string>> GeneratePositionsReport(Guid projectId);
    Task<Response<string>> GenerateB1Report(Guid projectId);
    Task<Response<string>> GenerateB2Report(Guid projectId);
}