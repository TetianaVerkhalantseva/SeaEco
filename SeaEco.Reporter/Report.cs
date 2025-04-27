using Microsoft.Extensions.Options;
using OfficeOpenXml;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Extensions;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Reporter.Models;
using SeaEco.Reporter.Models.B1;

namespace SeaEco.Reporter;
/*
public sealed class Report
{
    private const string DocumentNotFoundError = "Document not found";

    private readonly ReportOptions _options;

    public Report(IOptionsMonitor<ReportOptions> optionsMonitor)
    {
        _options = optionsMonitor.CurrentValue;
    }

    public Response<string> CopyDocument(SheetName name)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(_options.TemplatePath);

        ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[name.GetDescription()];
        if (sourceSheet is null)
        {
            return Response<string>.Error(DocumentNotFoundError);
        }

        string newPeportName = $"{name.GetDescription()}-{Guid.NewGuid()}";

        using var destinationPackage = new ExcelPackage();
        destinationPackage.Workbook.Worksheets.Add(sourceSheet.Name, sourceSheet);
        destinationPackage.SaveAs(new FileInfo(newPeportName));

        return Response<string>.Ok(newPeportName);
    }

    public Response FillB1(string path, List<ColumnB1> columns)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(_options.TemplatePath);
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        for (int i = 0; i < columns.Count && i < 10; i++)
        {
            int column = i + 4;

            worksheet.Cells[5, column].Value = columns[i].Bunntype.GetDescription();
            worksheet.Cells[7, column].Value = (int)columns[i].Dyr;

        }
    }
}
*/