using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Enums.Bsensorisk;
using SeaEco.Abstractions.Extensions;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Reporter.Models;
using SeaEco.Reporter.Models.B1;
using SeaEco.Reporter.Models.B2;
using SeaEco.Reporter.Models.Info;
using System.Drawing;

namespace SeaEco.Reporter;

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

        string newPeportName = $"{name.GetDescription()}-{Guid.NewGuid()}.xlsx";

        using var destinationPackage = new ExcelPackage();
        destinationPackage.Workbook.Worksheets.Add(sourceSheet.Name, sourceSheet);

        try
        {
            destinationPackage.SaveAs(new FileInfo(Path.Combine(_options.DestinationPath, newPeportName)));
        }
        catch (Exception ex)
        {
            return Response<string>.Error(ex.ToString());
        }
        return Response<string>.Ok(newPeportName);
    }

    public void FillB1(string path, IEnumerable<ColumnB1> columns, BHeader header, TilstandB1 tilstand, SjovannB1 sjovann)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        worksheet.Cells[1, 6].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 11].Value = string.Join(',', header.FeltDatoer.Select(date => date.ToString("dd.MM.yy"))); 
        worksheet.Cells[2, 6].Value = header.Lokalitetsnavn;
        worksheet.Cells[2, 11].Value = header.LokalitetsID;

        worksheet.Cells[1, 21].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 26].Value = string.Join(',', header.FeltDatoer.Select(date => date.ToString("dd.MM.yy")));
        worksheet.Cells[2, 21].Value = header.Lokalitetsnavn;
        worksheet.Cells[2, 26].Value = header.LokalitetsID;

        worksheet.Cells["H14"].Value = sjovann.SjoTemperatur;
        worksheet.Cells["J14"].Value = sjovann.SjoTemperatur;
        worksheet.Cells["H15"].Value = sjovann.pHSjo;
        worksheet.Cells["J15"].Value = sjovann.EhSjo;
        worksheet.Cells["M14"].Value = sjovann.SedimentTemperatur;
        worksheet.Cells["M15"].Value = sjovann.RefElektrode;
        
        int index = 4;
        foreach (ColumnB1 column in columns)
        {
            worksheet.Cells[5, index].Value = column.Bunntype.GetDisplay();
            worksheet.Cells[7, index].Value = (int)column.Dyr;

            worksheet.Cells[9, index].Value = column.Bunntype == Bunntype.Hardbunn || column.pH == 0 ? string.Empty : $"{column.pH:F1}";
            worksheet.Cells[10, index].Value = column.Bunntype == Bunntype.Hardbunn || column.Eh == 0 ? string.Empty : $"{column.Eh:F1}";
            worksheet.Cells[11, index].Value = column.Bunntype == Bunntype.Hardbunn ? 0 : column.phEh;
            
            if (Enum.IsDefined(column.TilstandProveGr2))
            {
                worksheet.Cells[12, index].Value = (int)column.TilstandProveGr2;
                worksheet.Cells[12, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[12, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(column.TilstandProveGr2.GetDisplay()));
            }
            
            
            worksheet.Cells[column.Gassbobler == Gassbobler.Ja ? 17 : 18, index].Value = (int)column.Gassbobler;
            worksheet.Cells[column.Farge == Farge.LysGrå ? 19 : 20, index].Value = (int)column.Farge;
            
            
            worksheet.Cells[column.Lukt switch 
            { 
                Lukt.Ingen => 21,
                Lukt.Noe=> 22,
                Lukt.Sterk => 23,
                _ => 21
            }, index].Value = (int)column.Lukt;

            worksheet.Cells[column.Konsistens switch 
            { 
                Konsistens.Fast => 24,
                Konsistens.Myk => 25,
                Konsistens.Løs => 26,
                _ => 24
            }, index].Value = (int)column.Konsistens;

            worksheet.Cells[column.Grabbvolum switch
            {
                Grabbvolum.MindreEnnKvart => 27,
                Grabbvolum.MellomKvartOgTreKvart => 28,
                Grabbvolum.StørreEnnTreKvart => 29,
                _ => 27
            }, index].Value = (int)column.Grabbvolum;

            worksheet.Cells[column.Tykkelseslamlag switch
            {
                Tykkelseslamlag.Under2Cm => 30,
                Tykkelseslamlag.Mellom2Og8Cm => 31,
                Tykkelseslamlag.Over8Cm => 32,
                _ => 30
            }, index].Value = (int)column.Tykkelseslamlag;

            worksheet.Cells[33, index].Value = column.Sum > 0 ? $"{column.Sum:F2}" : 0;
            worksheet.Cells[34, index].Value = column.KorrigertSum;

            if (Enum.IsDefined(typeof(Tilstand), column.TilstandProveGr3))
            {
                worksheet.Cells[35, index].Value = (int)column.TilstandProveGr3;
                worksheet.Cells[35, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[35, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(column.TilstandProveGr3.GetDisplay()));
            }

            worksheet.Cells[38, index].Value = column.MiddelVerdiGr2Gr3 > 0 ? $"{column.MiddelVerdiGr2Gr3:F2}" : 0;

            
            
            if (Enum.IsDefined(typeof(Tilstand), column.TilstandProveGr2Gr3))
            {
                worksheet.Cells[39, index].Value = (int)column.TilstandProveGr2Gr3;
                worksheet.Cells[39, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[39, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(column.TilstandProveGr2Gr3.GetDisplay()));
            }

            index = index % 13 == 0 ? 19 : index + 1;
        }

        worksheet.Cells["AC11"].Value = tilstand.IndeksGr2;
        
        worksheet.Cells["S13"].Value = (int)tilstand.TilstandGr2;
        worksheet.Cells["S13"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells["S13"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.TilstandGr2.GetDisplay()));
        
        worksheet.Cells["AC34"].Value = tilstand.IndeksGr3;
        
        worksheet.Cells["S36"].Value = (int)tilstand.TilstandGr3;
        worksheet.Cells["S36"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells["S36"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.TilstandGr3.GetDisplay()));

        worksheet.Cells["AC38"].Value = tilstand.LokalitetsIndeks;
        
        worksheet.Cells["Z41"].Value = (int)tilstand.LokalitetsTilstand;
        worksheet.Cells["Z41"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells["Z41"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.LokalitetsTilstand.GetDisplay()));

        sourcePackage.Save();
    }

    public void FillB2(string path, IEnumerable<ColumnB2> columns, BHeader header)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        int index = 3;

        worksheet.Cells[1, 4].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 9].Value = string.Join(',', header.FeltDatoer.Select(date => date.ToString("dd.MM.yy")));
        worksheet.Cells[2, 4].Value = header.Lokalitetsnavn;
        worksheet.Cells[2, 9].Value = header.LokalitetsID;

        worksheet.Cells[1, 17].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 22].Value = string.Join(',', header.FeltDatoer.Select(date => date.ToString("dd.MM.yy")));
        worksheet.Cells[2, 17].Value = header.Lokalitetsnavn;
        worksheet.Cells[2, 22].Value = header.LokalitetsID;

        foreach (ColumnB2 column in columns)
        {
            worksheet.Cells[5, index].Value = column.KoordinatNord;
            worksheet.Cells[6, index].Value = column.KoordinatOst;
            worksheet.Cells[7, index].Value = column.Dyp;
            worksheet.Cells[8, index].Value = column.AntallGrabbhugg;
            worksheet.Cells[9, index].Value = column.Bobling ? "x" : string.Empty;

            worksheet.Cells[11, index].Value = column.Leire.Key.GetDescription();
            worksheet.Cells[12, index].Value = column.Silt.Key.GetDescription();
            worksheet.Cells[13, index].Value = column.Sand.Key.GetDescription();
            worksheet.Cells[14, index].Value = column.Grus.Key.GetDescription();
            worksheet.Cells[15, index].Value = column.Skjellsand.Key.GetDescription();
            worksheet.Cells[16, index].Value = column.Steinbunn.Key.GetDescription();
            worksheet.Cells[17, index].Value = column.Fjellbunn.Key.GetDescription();

            worksheet.Cells[19, index].Value = column.Pigghuder;
            worksheet.Cells[20, index].Value = column.Krepsdyr;
            worksheet.Cells[21, index].Value = column.Skjell;
            worksheet.Cells[22, index].Value = column.Børstemark;
            worksheet.Cells[23, index].Value = string.Empty;

            worksheet.Cells[24, index].Value = column.Beggiota ? "x" : string.Empty;
            worksheet.Cells[25, index].Value = column.Fôr ? "x" : string.Empty;
            worksheet.Cells[26, index].Value = column.Fekalier ? "x" : string.Empty;

            worksheet.Cells[27, index].Value = column.Kommentarer;
        }
        
        sourcePackage.Save();
    }

    public void FillInfo(string path, CommonInformation information)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        worksheet.Cells[1, 2].Value = information.ProsjektIdSe;
        worksheet.Cells[2, 2].Value = information.FeltDatoer;

        worksheet.Cells[5, 2].Value = information.TotalStasjoner;
        worksheet.Cells[6, 2].Value = information.TotalGrabbhugg;
        worksheet.Cells[7, 2].Value = information.Hardbunnsstasjoner;
        worksheet.Cells[8, 2].Value = information.MedDyr;
        worksheet.Cells[9, 2].Value = information.MedPhEh;

        worksheet.Cells[12, 2].Value = $"{information.Leire:F1}";
        worksheet.Cells[13, 2].Value = $"{information.Silt:F1}";
        worksheet.Cells[14, 2].Value = $"{information.Sand:F1}";
        worksheet.Cells[15, 2].Value = $"{information.Grus:F1}";
        worksheet.Cells[16, 2].Value = $"{information.Skjellsand:F1}";
        worksheet.Cells[17, 2].Value = $"{information.Steinbunn:F1}";
        worksheet.Cells[18, 2].Value = $"{information.Fjellbunn:F1}";

        worksheet.Cells[21, 2].Value = information.Tilstand1.Item1 == 0 ? "-" : information.Tilstand1.Item1;
        worksheet.Cells[21, 3].Value = information.Tilstand1.Item2 == 0 ? "-" : information.Tilstand1.Item2;

        worksheet.Cells[22, 2].Value = information.Tilstand2.Item1 == 0 ? "-" : information.Tilstand2.Item1;
        worksheet.Cells[22, 3].Value = information.Tilstand2.Item2 == 0 ? "-" : information.Tilstand2.Item2;

        worksheet.Cells[23, 2].Value = information.Tilstand3.Item1 == 0 ? "-" : information.Tilstand3.Item1;
        worksheet.Cells[23, 3].Value = information.Tilstand3.Item2 == 0 ? "-" : information.Tilstand3.Item2;
        
        worksheet.Cells[24, 2].Value = information.Tilstand4.Item1 == 0 ? "-" : information.Tilstand4.Item1;
        worksheet.Cells[24, 3].Value = information.Tilstand4.Item2 == 0 ? "-" : information.Tilstand4.Item2;
    }
}