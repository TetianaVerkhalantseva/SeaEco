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
using OfficeOpenXml.Drawing;
using SeaEco.Reporter.Models.Headers;
using SeaEco.Reporter.Models.Images;
using SeaEco.Reporter.Models.Plot;
using SeaEco.Reporter.Models.Positions;
using SeaEco.Reporter.Models.PTP;

namespace SeaEco.Reporter;


public sealed class Report
{
    private const string DocumentNotFoundError = "Document not found";
    private const string ErrorWhileDownloadingDocument = "Error while downloading document";

    private readonly ReportOptions _options;

    public Report(IOptionsMonitor<ReportOptions> optionsMonitor)
    {
        _options = optionsMonitor.CurrentValue;
    }

    public Response<string> CopyDocument(string projectIdSe, SheetName name)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(_options.TemplatePath);

        ExcelWorksheet sourceSheet = sourcePackage.Workbook.Worksheets[name.GetDescription()];
        if (sourceSheet is null)
        {
            return Response<string>.Error(DocumentNotFoundError);
        }

        string newPeportName = $"{projectIdSe}-{name.GetDescription()}.xlsx";
        string fullPath = Path.Combine(_options.DestinationPath, newPeportName);
        string? directory = Path.GetDirectoryName(fullPath);

        using var destinationPackage = new ExcelPackage();
        destinationPackage.Workbook.Worksheets.Add(sourceSheet.Name, sourceSheet);

        try
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            destinationPackage.SaveAs(new FileInfo(fullPath));
        }
        catch (Exception ex)
        {
            return Response<string>.Error(ex.ToString());
        }
        return Response<string>.Ok(newPeportName);
    }

    public Response<FileModel> DownloadReport(string projectIdSe, SheetName name)
    {
        string reportName = $"{projectIdSe}-{name.GetDescription()}.xlsx";
        if (!File.Exists(Path.Combine(_options.DestinationPath, reportName)))
        {
            return Response<FileModel>.Error(DocumentNotFoundError);
        }
        
        using FileStream fileStream = new FileStream(Path.Combine(_options.DestinationPath, reportName), FileMode.Open, FileAccess.Read);
        using MemoryStream memory = new MemoryStream();
        
        try
        {
            fileStream.CopyTo(memory);
        }
        catch
        {
            return Response<FileModel>.Error(ErrorWhileDownloadingDocument);
        }

        return Response<FileModel>.Ok(new FileModel()
        {
            Content = memory.ToArray(),
            DownloadName = reportName
        });
    }
    
    public void FillB1(string path, IEnumerable<ColumnB1> columns, BHeader header, TilstandB1 tilstand, SjovannB1 sjovann)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();
        
        if (int.TryParse(header.LokalitetsID, out int lokalitetsID))
        {
            worksheet.Cells[2, 11].Value = lokalitetsID;
            worksheet.Cells[2, 26].Value = lokalitetsID;
        }
        else
        {
            worksheet.Cells[2, 11].Value = header.LokalitetsID;
            worksheet.Cells[2, 26].Value = header.LokalitetsID;
        }

        worksheet.Cells[1, 6].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 11].Value = string.Join(", ", header.FeltDatoer.Select(date => date.ToString("dd.MM.yy"))); 
        worksheet.Cells[2, 6].Value = header.Lokalitetsnavn;

        worksheet.Cells[1, 21].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 26].Value = string.Join(", ", header.FeltDatoer.Select(date => date.ToString("dd.MM.yy")));
        worksheet.Cells[2, 21].Value = header.Lokalitetsnavn;
        
        int index = 4;
        foreach (ColumnB1 column in columns)
        {
            worksheet.Cells[4, index].Value = column.Nummer;
            
            worksheet.Cells[5, index].Value = column.Bunntype.GetDisplay();
            worksheet.Cells[7, index].Value = (int)column.Dyr;

            if (!(column.Bunntype == Bunntype.Hardbunn &&
                column.HasSediment == false &&
                column.HasSensorisk == true))
            {
                worksheet.Cells[9, index].Value = column.Bunntype == Bunntype.Hardbunn || column.pH == 0 ? string.Empty : $"{column.pH:F1}";
                worksheet.Cells[10, index].Value = column.Bunntype == Bunntype.Hardbunn || column.Eh == 0 ? string.Empty : $"{column.Eh:F1}";
                worksheet.Cells[11, index].Value = column.Bunntype == Bunntype.Hardbunn ? 0 : column.phEh;
            
                if (Enum.IsDefined(column.TilstandProveGr2))
                {
                    worksheet.Cells[12, index].Value = (int)column.TilstandProveGr2;
                    worksheet.Cells[12, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[12, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(column.TilstandProveGr2.GetDisplay()));
                }
                else if (column.TilstandProveGr2 == 0)
                {
                    Tilstand gr2 = Tilstand.Blue;

                    worksheet.Cells[12, index].Value = (int)gr2;
                    worksheet.Cells[12, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[12, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(gr2.GetDisplay()));
                }
            }

            if (column.HasSensorisk)
            {
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
            }

            worksheet.Cells[33, index].Value = column.Sum;
            worksheet.Cells[34, index].Value = column.KorrigertSum;

            if (Enum.IsDefined(typeof(Tilstand), column.TilstandProveGr3))
            {
                worksheet.Cells[35, index].Value = (int)column.TilstandProveGr3;
                worksheet.Cells[35, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[35, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(column.TilstandProveGr3.GetDisplay()));
            }
            else if (column.KorrigertSum == 0)
            {
                Tilstand gr3 = Tilstand.Blue;

                worksheet.Cells[35, index].Value = (int)gr3;
                worksheet.Cells[35, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[35, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(gr3.GetDisplay()));
            }

            worksheet.Cells[38, index].Value = column.MiddelVerdiGr2Gr3 > 0 ? $"{column.MiddelVerdiGr2Gr3:F2}" : 0;
            
            
            if (Enum.IsDefined(typeof(Tilstand), column.TilstandProveGr2Gr3)) 
            {
                worksheet.Cells[39, index].Value = (int)column.TilstandProveGr2Gr3;
                worksheet.Cells[39, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[39, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(column.TilstandProveGr2Gr3.GetDisplay()));
            }
            else if (column.MiddelVerdiGr2Gr3 == 0)
            {
                Tilstand gr2_3 = Tilstand.Blue;
                
                worksheet.Cells[39, index].Value = (int)gr2_3;
                worksheet.Cells[39, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[39, index].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(gr2_3.GetDisplay()));
            }

            index = index % 13 == 0 ? 19 : index + 1;
        }

        worksheet.Cells["H14"].Value = sjovann.SjoTemperatur;
        worksheet.Cells["J14"].Value = sjovann.SjoTemperatur;
        worksheet.Cells["H15"].Value = sjovann.pHSjo;
        worksheet.Cells["J15"].Value = sjovann.EhSjo;
        worksheet.Cells["M14"].Value = sjovann.SedimentTemperatur;
        worksheet.Cells["M15"].Value = sjovann.RefElektrode;

        if (columns.Count() > 10)
        {
            worksheet.Cells["AC11"].Value = tilstand.IndeksGr2;

            int til2 = (int)tilstand.TilstandGr2;
            if (til2 != 0)
            {
                worksheet.Cells["S13"].Value = til2;
                worksheet.Cells["S13"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["S13"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.TilstandGr2.GetDisplay()));
            }
        
            worksheet.Cells["AC34"].Value = tilstand.IndeksGr3;

            int til3 = (int)tilstand.TilstandGr3;
            if (til3 != 0)
            {
                worksheet.Cells["S36"].Value = til3;
                worksheet.Cells["S36"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["S36"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.TilstandGr3.GetDisplay()));
            }

            worksheet.Cells["AC38"].Value = tilstand.LokalitetsIndeks;

            int tilLok = (int)tilstand.LokalitetsTilstand;
            if (tilLok != 0)
            {
                worksheet.Cells["Z41"].Value = tilLok;
                worksheet.Cells["Z41"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["Z41"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.LokalitetsTilstand.GetDisplay()));
            }   
        }
        else
        {
            worksheet.Cells["N11"].Value = tilstand.IndeksGr2;

            int til2 = (int)tilstand.TilstandGr2;
            if (til2 != 0)
            {
                worksheet.Cells["D13"].Value = til2;
                worksheet.Cells["D13"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["D13"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.TilstandGr2.GetDisplay()));
            }
        
            worksheet.Cells["N34"].Value = tilstand.IndeksGr3;

            int til3 = (int)tilstand.TilstandGr3;
            if (til3 != 0)
            {
                worksheet.Cells["D36"].Value = til3;
                worksheet.Cells["D36"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["D36"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.TilstandGr3.GetDisplay()));
            }

            worksheet.Cells["N38"].Value = tilstand.LokalitetsIndeks;

            int tilLok = (int)tilstand.LokalitetsTilstand;
            if (tilLok != 0)
            {
                worksheet.Cells["K41"].Value = tilLok;
                worksheet.Cells["K41"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["K41"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(tilstand.LokalitetsTilstand.GetDisplay()));
            }
        }

        sourcePackage.Save();
    }

    public void FillB2(string path, IEnumerable<ColumnB2> columns, BHeader header)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        int index = 3;

        if (int.TryParse(header.LokalitetsID, out int lokalitetsID))
        {
            worksheet.Cells[2, 9].Value = lokalitetsID;
            worksheet.Cells[2, 22].Value = lokalitetsID;
        }
        else
        {
            worksheet.Cells[2, 9].Value = header.LokalitetsID;
            worksheet.Cells[2, 22].Value = header.LokalitetsID;
        }
        
        worksheet.Cells[1, 4].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 9].Value = string.Join(", ", header.FeltDatoer.Select(date => date.ToString("dd.MM.yy")));
        worksheet.Cells[2, 4].Value = header.Lokalitetsnavn;

        worksheet.Cells[1, 17].Value = header.Oppdragsgiver;
        worksheet.Cells[1, 22].Value = string.Join(", ", header.FeltDatoer.Select(date => date.ToString("dd.MM.yy")));
        worksheet.Cells[2, 17].Value = header.Lokalitetsnavn;

        foreach (ColumnB2 column in columns)
        {
            worksheet.Cells[4, index].Value = column.Nummer;
            
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

            index++;
        }
        
        sourcePackage.Save();
    }

    public void FillInfo(string path, CommonInformation information)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        worksheet.Cells[1, 2].Value = information.ProsjektIdSe;
        worksheet.Cells[2, 2].Value = string.Join(", ", information.FeltDatoer.Select(_ => _.ToString("dd.MM.yy")));

        worksheet.Cells[5, 2].Value = information.TotalStasjoner;
        worksheet.Cells[6, 2].Value = information.TotalGrabbhugg;
        worksheet.Cells[7, 2].Value = information.Hardbunnsstasjoner;
        worksheet.Cells[8, 2].Value = information.MedDyr;
        worksheet.Cells[9, 2].Value = information.MedPhEh;

        worksheet.Cells[12, 2].Value = information.Leire;
        worksheet.Cells[13, 2].Value = information.Silt;
        worksheet.Cells[14, 2].Value = information.Sand;
        worksheet.Cells[15, 2].Value = information.Grus;
        worksheet.Cells[16, 2].Value = information.Skjellsand;
        worksheet.Cells[17, 2].Value = information.Steinbunn;
        worksheet.Cells[18, 2].Value = information.Fjellbunn;

        worksheet.Cells[21, 2].Value = information.Tilstand1.Item1 == 0 ? "-" : information.Tilstand1.Item1;
        worksheet.Cells[21, 3].Value = information.Tilstand1.Item2 == 0 ? "-" : information.Tilstand1.Item2;

        worksheet.Cells[22, 2].Value = information.Tilstand2.Item1 == 0 ? "-" : information.Tilstand2.Item1;
        worksheet.Cells[22, 3].Value = information.Tilstand2.Item2 == 0 ? "-" : information.Tilstand2.Item2;

        worksheet.Cells[23, 2].Value = information.Tilstand3.Item1 == 0 ? "-" : information.Tilstand3.Item1;
        worksheet.Cells[23, 3].Value = information.Tilstand3.Item2 == 0 ? "-" : information.Tilstand3.Item2;
        
        worksheet.Cells[24, 2].Value = information.Tilstand4.Item1 == 0 ? "-" : information.Tilstand4.Item1;
        worksheet.Cells[24, 3].Value = information.Tilstand4.Item2 == 0 ? "-" : information.Tilstand4.Item2;

        worksheet.Cells[27, 2].Value = $"{information.IndeksGr2:F2}";
        if (Enum.IsDefined<Tilstand>(information.TilstandGr2))
        {
            worksheet.Cells[27, 3].Value = (int)information.TilstandGr2;
            worksheet.Cells[27, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[27, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(information.TilstandGr2.GetDisplay()));
        }

        worksheet.Cells[28, 2].Value = $"{information.IndeksGr3:F2}";
        if (Enum.IsDefined<Tilstand>(information.TilstandGr3))
        {
            worksheet.Cells[28, 3].Value = (int)information.TilstandGr3;
            worksheet.Cells[28, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[28, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(information.TilstandGr3.GetDisplay()));
        }

        worksheet.Cells[29, 2].Value = $"{information.LokalitetsIndeks:F2}";
        if (Enum.IsDefined<Tilstand>(information.LokalitetsTilstand))
        {
            worksheet.Cells[29, 3].Value = (int)information.LokalitetsTilstand;
            worksheet.Cells[29, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[29, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(information.LokalitetsTilstand.GetDisplay()));
        }
            
        sourcePackage.Save();
    }

    public void FillPositions(string path, IEnumerable<RowPosition> positions)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        int index = 2;
        foreach (RowPosition position in positions)
        {
            worksheet.Cells[index, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[index, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(1, 205, 221, 225));
            worksheet.Cells[index, 1].Value = position.Nummer;
            
            worksheet.Cells[index, 2].Value = position.KoordinatNord;
            worksheet.Cells[index, 3].Value = position.KoordinatOst;
            worksheet.Cells[index, 4].Value = position.Dybde;
            worksheet.Cells[index, 5].Value = position.AntallGrabbhugg;
            worksheet.Cells[index, 6].Value = position.Bunntype.GetDisplay();

            Border border = worksheet.Cells[index, 1, index, 6].Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;

            index++;
        }

        sourcePackage.Save();
    }
    
    public void FillPtp(string path, IEnumerable<RowPtp> ptps, PtpHeader header)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        worksheet.Cells["B6"].Value = header.Lokalitetsnavn;
        worksheet.Cells["B7"].Value = header.Oppdragsgiver;
        worksheet.Cells["F6"].Value = header.Planlagtfeltdato.ToString("dd.MM.yyyy");
        worksheet.Cells["F7"].Value = header.Planlegger;
        
        int index = 10;
        foreach (RowPtp ptp in ptps)
        {
            worksheet.Cells[index, 1].Value = ptp.Planlagtfeltdato.ToString("dd.MM.yyyy");
            worksheet.Cells[index, 2].Value = ptp.Nummer;
            
            worksheet.Cells[index, 3].Value = ptp.KoordinatNord;
            worksheet.Cells[index, 4].Value = "N";
            worksheet.Cells[index, 5].Value = ptp.KoordinatOst;
            worksheet.Cells[index, 6].Value = "Ø";
            worksheet.Cells[index, 7].Value = ptp.Dybde;
            worksheet.Cells[index, 8].Value = ptp.Analyser;

            Border border = worksheet.Cells[index, 1, index, 8].Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;

            index++;
        }

        sourcePackage.Save();
    }
    
    public void FillImages(string path, IEnumerable<RowImage> images)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        int index = 6;
        foreach (RowImage image in images)
        {
            worksheet.Cells[index, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[index, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(1, 218, 237, 243));
            worksheet.Cells[index, 1].Value = image.Nummer;

            int pixelHeight = (int)(281 * 1.33);
            int pixelWidth = (int)(35.5 * 7 + 5);
            
            if (image.UsiltImage is not null && image.UsiltImage.Length > 0)
            {
                ExcelPicture usiltImage = worksheet.Drawings.AddPicture($"Usilt_{Guid.NewGuid()}", new MemoryStream(image.UsiltImage));
                usiltImage.SetPosition(index - 1, 0, 2 - 1, 0);
                usiltImage.SetSize(pixelWidth, pixelHeight);
            }
            else
            {
                worksheet.Cells[index, 2].Value = "Tom grabb";
            }

            if (image.SiltImage is not null && image.SiltImage.Length > 0)
            {
                ExcelPicture siltImage = worksheet.Drawings.AddPicture($"Silt_{Guid.NewGuid()}", new MemoryStream(image.SiltImage));
                siltImage.SetPosition(index - 1, 0, 3 - 1, 0);
                siltImage.SetSize(pixelWidth, pixelHeight);
            }
            else
            {
                worksheet.Cells[index, 3].Value = "For lite sediment – prøve ikke silt";
            }

            Border border = worksheet.Cells[index, 1, index, 3].Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;

            index++;
        }

        sourcePackage.Save();
    }

    public void FillPhEhPlot(string path, IEnumerable<PlotColumn> columns)
    {
        ExcelPackage.License.SetNonCommercialPersonal(_options.NonCommercialPersonalName);

        using ExcelPackage sourcePackage = new ExcelPackage(Path.Combine(_options.DestinationPath, path));
        using ExcelWorksheet worksheet = sourcePackage.Workbook.Worksheets.First();

        int index = 3;
        foreach (PlotColumn column in columns)
        {
            worksheet.Cells[1, index].Value = column.Ph;
            worksheet.Cells[2, index].Value = column.Eh;
            index++;
        }
        
        sourcePackage.Save();
    }
}
