using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.ReportServices;

public interface IReportService
{
    Task<Response<string>> GenerateInfoReport(Guid projectId);
    Task<Response<string>> GenerateB1Report(Guid projectId);
}