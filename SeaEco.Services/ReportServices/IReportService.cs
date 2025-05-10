using SeaEco.Abstractions.Models.Report;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.Reporter.Models;

namespace SeaEco.Services.ReportServices;

public interface IReportService
{
    Task<Response<string>> GenerateInfoReport(Guid projectId);
    Task<Response<string>> GeneratePositionsReport(Guid projectId);
    Task<Response<string>> GenerateB1Report(Guid projectId);
    Task<Response<string>> GenerateB2Report(Guid projectId);
    Task<IEnumerable<Response<string>>> GenerateAllReports(Guid projectId);
    
    Task<IEnumerable<ReportDto>> GetAllReports(Guid projectId);
    Task<Response<FileModel>> DownloadReportById(Guid reporId);
}