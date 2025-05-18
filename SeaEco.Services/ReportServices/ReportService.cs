using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Enums.Bsensorisk;
using SeaEco.Abstractions.Extensions;
using SeaEco.Abstractions.Models.Report;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Abstractions.ValueObjects.Bunnsubstrat;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Reporter;
using SeaEco.Reporter.Models;
using SeaEco.Reporter.Models.B1;
using SeaEco.Reporter.Models.B2;
using SeaEco.Reporter.Models.Headers;
using SeaEco.Reporter.Models.Images;
using SeaEco.Reporter.Models.Info;
using SeaEco.Reporter.Models.Plot;
using SeaEco.Reporter.Models.Positions;
using SeaEco.Reporter.Models.PTP;
using SeaEco.Services.TilstandServices;

namespace SeaEco.Services.ReportServices;

public sealed class ReportService(Report report,
    TilstandService tilstandService,
    IGenericRepository<BProsjekt> projectRepository,
    IGenericRepository<BRapporter> reportRepository,
    IGenericRepository<BSediment> sedimentRepository,
    IGenericRepository<BSensorisk> sensoriskRepository,
    IGenericRepository<BUndersokelse> undersokelseRepository,
    IGenericRepository<BTilstand> tilstandRepository,
    IWebHostEnvironment webHostEnvironment)
    : IReportService
{
    private const string ProjectNotFoundError = "Project not found";
    private const string ReportNotFoundError = "Report not found";
    private const string InvalidReportGenerationData = "Invalid report generation data";

    public async Task<Response<string>> GenerateInfoReport(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(project => project.BTilstand)
            .Include(project => project.BPreinfos)
            .Include(project => project.BStasjons)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Blotbunn)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Hardbunn)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Dyr)
            .FirstOrDefaultAsync(project => project.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.Info);
        if (copyResult.IsError)
        {
            return copyResult;
        }
        
        IEnumerable<BUndersokelse> undersokelses = dbRecord.BUndersokelses;

        report.FillInfo(copyResult.Value, new CommonInformation()
        {
            ProsjektIdSe = dbRecord.ProsjektIdSe ?? string.Empty,
            FeltDatoer = dbRecord.BPreinfos.OrderBy(_ => _.Feltdato).Select(_ => _.Feltdato),

            TotalStasjoner = dbRecord.BStasjons.Count(_ => _.UndersokelseId is not null),
            TotalGrabbhugg = undersokelses.Sum(_ => _.AntallGrabbhugg ?? 0),
            Hardbunnsstasjoner = undersokelses.Count(_ => _.HardbunnId is not null),
            MedDyr = undersokelses.Count(_ => _.DyrId is not null),
            MedPhEh = undersokelses.Count(_ => _.SedimentId is not null),
            
            Leire = undersokelses.Sum(_ => _.Blotbunn?.Leire ?? 0),
            Silt = undersokelses.Sum(_ => _.Blotbunn?.Silt ?? 0),
            Sand = undersokelses.Sum(_ => _.Blotbunn?.Sand ?? 0),
            Grus = undersokelses.Sum(_ => _.Blotbunn?.Grus ?? 0),
            Skjellsand = undersokelses.Sum(_ => _.Blotbunn?.Skjellsand ?? 0),
            Steinbunn = undersokelses.Sum(_ => _.Hardbunn?.Steinbunn ?? 0),
            Fjellbunn = undersokelses.Sum(_ => _.Hardbunn?.Fjellbunn ?? 0),

            Tilstand1 = Tuple.Create(undersokelses.Where(_ => _.TilstandGr2Gr3 == 1).Count(), undersokelses.Where(_ => _.TilstandGr2Gr3 == 1 && _.HardbunnId is not null).Count()),
            Tilstand2 = Tuple.Create(undersokelses.Where(_ => _.TilstandGr2Gr3 == 2).Count(), undersokelses.Where(_ => _.TilstandGr2Gr3 == 2 && _.HardbunnId is not null).Count()),
            Tilstand3 = Tuple.Create(undersokelses.Where(_ => _.TilstandGr2Gr3 == 3).Count(), undersokelses.Where(_ => _.TilstandGr2Gr3 == 3 && _.HardbunnId is not null).Count()),
            Tilstand4 = Tuple.Create(undersokelses.Where(_ => _.TilstandGr2Gr3 == 4).Count(), undersokelses.Where(_ => _.TilstandGr2Gr3 == 4 && _.HardbunnId is not null).Count()),

            IndeksGr2 = dbRecord.BTilstand?.IndeksGr2 ?? 0,
            IndeksGr3 = dbRecord.BTilstand?.IndeksGr3 ?? 0,
            LokalitetsIndeks = dbRecord.BTilstand?.IndeksLokalitet ?? 0,
            TilstandGr2 = (Tilstand)(dbRecord.BTilstand?.TilstandGr2 ?? 0),
            TilstandGr3 = (Tilstand)(dbRecord.BTilstand?.TilstandGr3 ?? 0),
            LokalitetsTilstand = (Tilstand)(dbRecord.BTilstand?.TilstandLokalitet ?? 0)
        });

        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.Info);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }
    
    public async Task<Response<string>> GenerateB1Report(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(project => project.Kunde)
            .Include(project => project.BPreinfos)
            .Include(project => project.Lokalitet)
            .Include(project => project.BTilstand)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Sediment)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Sensorisk)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.BStasjon)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.B1);
        if (copyResult.IsError)
        {
            return copyResult;
        }
        
        IEnumerable<ColumnB1> columns = dbRecord.BUndersokelses.OrderBy(_ => _.BStasjon!.Nummer).Select(_ => new ColumnB1()
        {
            Nummer = _.BStasjon.Nummer,
            
            Bunntype = _.HardbunnId is null ? Bunntype.Blotbunn : Bunntype.Hardbunn,
            Dyr = _.DyrId is null ? Dyr.Nei : Dyr.Ja,
            
            HasSediment = _.SedimentId is not null,
            pH = _.Sediment?.Ph ?? 0,
            Eh = _.Sediment?.Eh ?? 0,
            phEh = _.Sediment?.KlasseGr2 ?? 0,
            TilstandProveGr2 = (Tilstand)(_.Sediment?.TilstandGr2 ?? 1),
            
            HasSensorisk = _.SensoriskId is not null,
            Gassbobler = (Gassbobler)(_.Sensorisk?.Gassbobler ?? 0),
            Farge = (Farge)(_.Sensorisk?.Farge ?? 0),
            Lukt = (Lukt)(_.Sensorisk?.Lukt ?? 0),
            Konsistens = (Konsistens)(_.Sensorisk?.Konsistens ?? 0),
            Grabbvolum = (Grabbvolum)(_.Sensorisk?.Grabbvolum ?? 0),
            Tykkelseslamlag = (Tykkelseslamlag)(_.Sensorisk?.Tykkelseslamlag ?? 0),
            Sum = _.SensoriskId is null ? 0
                : _.Sensorisk.Gassbobler +
                  _.Sensorisk.Farge +
                  _.Sensorisk.Lukt +
                  _.Sensorisk.Konsistens +
                  _.Sensorisk.Grabbvolum +
                  _.Sensorisk.Tykkelseslamlag,
            KorrigertSum = _.Sensorisk?.IndeksGr3 ?? 0,
            TilstandProveGr3 = (Tilstand)(_.Sensorisk?.TilstandGr3 ?? 1),
            MiddelVerdiGr2Gr3 = _.IndeksGr2Gr3 ?? 0,
            TilstandProveGr2Gr3 = (Tilstand)(_.TilstandGr2Gr3 ?? 1)
        });

        BHeader header = new BHeader()
        {
            Oppdragsgiver = dbRecord.Kunde.Oppdragsgiver,
            FeltDatoer = dbRecord.BPreinfos.OrderBy(_ => _.Feltdato).Select(_ => _.Feltdato),
            Lokalitetsnavn = dbRecord.Lokalitet.Lokalitetsnavn,
            LokalitetsID = dbRecord.Lokalitet.LokalitetsId
        };
        
        BPreinfo? dbPreinfo = dbRecord.BPreinfos.OrderBy(_ => _.Feltdato).FirstOrDefault();
        
        SjovannB1 sjovann = new SjovannB1()
        {
            SjoTemperatur = dbPreinfo?.SjoTemperatur ?? 0,
            pHSjo = dbPreinfo?.PhSjo ?? 0,
            EhSjo = dbPreinfo?.EhSjo ?? 0,
            SedimentTemperatur = dbRecord.BUndersokelses.Any() ? 
                dbRecord.BUndersokelses.Sum(u => u.Sediment?.Temperatur ?? 0) /
                dbRecord.BUndersokelses.Count(u => u.SedimentId is not null)
                : 0,
            RefElektrode = dbPreinfo?.RefElektrode ?? 0,
        };

        TilstandB1 tilstand = new TilstandB1()
        {
            IndeksGr2 = dbRecord.BTilstand?.IndeksGr2 ?? 0,
            TilstandGr2 = (Tilstand)(dbRecord.BTilstand?.TilstandGr2 ?? 0),
            IndeksGr3 = dbRecord.BTilstand?.IndeksGr3 ?? 0,
            TilstandGr3 = (Tilstand)(dbRecord.BTilstand?.TilstandGr3 ?? 0),
            LokalitetsIndeks = dbRecord.BTilstand?.IndeksLokalitet ?? 0,
            LokalitetsTilstand = (Tilstand)(dbRecord.BTilstand?.TilstandLokalitet ?? 0)
        };
        
        report.FillB1(copyResult.Value, columns, header, tilstand, sjovann);
        
        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.B1);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }

    public async Task<Response<string>> GenerateB2Report(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(project => project.Kunde)
            .Include(project => project.BPreinfos)
            .Include(project => project.Lokalitet)
            .Include(project => project.BUndersokelses)
                .ThenInclude(_ => _.Blotbunn)
            .Include(project => project.BUndersokelses)
                .ThenInclude(_ => _.BStasjon)
            .Include(project => project.BUndersokelses)
                .ThenInclude(_ => _.Hardbunn)
            .Include(project => project.BUndersokelses)
                .ThenInclude(_ => _.Sensorisk)
            .Include(project => project.BUndersokelses)
                .ThenInclude(_ => _.Dyr)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.B2);
        if (copyResult.IsError)
        {
            return copyResult;
        }

        IEnumerable<ColumnB2> columns = dbRecord.BUndersokelses.OrderBy(_ => _.BStasjon!.Nummer).Select(_ => new ColumnB2()
        {
            Nummer = _.BStasjon.Nummer,
            
            KoordinatNord = _.BStasjon.KoordinatNord,
            KoordinatOst = _.BStasjon.KoordinatOst,
            Dyp = _.BStasjon.Dybde,
            AntallGrabbhugg = _.AntallGrabbhugg ?? 0,
            Bobling = (Gassbobler)(_.Sensorisk?.Gassbobler ?? 0) == Gassbobler.Ja,
            
            Leire = new BunnsubstratValue(_.Blotbunn?.Leire ?? 0),
            Silt = new BunnsubstratValue(_.Blotbunn?.Silt ?? 0),
            Sand = new BunnsubstratValue(_.Blotbunn?.Sand ?? 0),
            Grus = new BunnsubstratValue(_.Blotbunn?.Grus ?? 0),
            Skjellsand = new BunnsubstratValue(_.Blotbunn?.Skjellsand ?? 0),
            Steinbunn = new BunnsubstratValue(_.Hardbunn?.Steinbunn ?? 0),
            Fjellbunn = new BunnsubstratValue(_.Hardbunn?.Fjellbunn ?? 0),
                
            Pigghuder = string.IsNullOrEmpty(_.Dyr?.Pigghunder) ? string.Empty : _.Dyr.Pigghunder,
            Krepsdyr = string.IsNullOrEmpty(_.Dyr?.Krepsdyr) ? string.Empty : _.Dyr.Krepsdyr,
            Skjell = string.IsNullOrEmpty(_.Dyr?.Skjell) ? string.Empty : _.Dyr.Skjell,
            Børstemark = string.IsNullOrEmpty(_.Dyr?.Borstemark) ? string.Empty : _.Dyr.Borstemark,
            
            Beggiota = _.Beggiatoa,
            Fôr = _.Forrester,
            Fekalier = _.Fekalier,
            
            Kommentarer = string.IsNullOrEmpty(_.Merknader) ? string.Empty : _.Merknader,
        });
        
        BHeader header = new BHeader()
        {
            Oppdragsgiver = dbRecord.Kunde.Oppdragsgiver,
            FeltDatoer = dbRecord.BPreinfos.OrderBy(_ => _.Feltdato).Select(_ => _.Feltdato),
            Lokalitetsnavn = dbRecord.Lokalitet.Lokalitetsnavn,
            LokalitetsID = dbRecord.Lokalitet.LokalitetsId
        };
        
        report.FillB2(copyResult.Value, columns, header);

        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.B2);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }

    public async Task<Response<string>> GeneratePositionsReport(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(_ => _.BStasjons)
                .ThenInclude(_ => _.Undersokelse)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.Position);
        if (copyResult.IsError)
        {
            return copyResult;
        }

        IEnumerable<RowPosition> positions = dbRecord.BStasjons.OrderBy(_ => _.Nummer).Select(_ => new RowPosition()
        {
            Nummer = _.Nummer,
            KoordinatNord = _.KoordinatNord,
            KoordinatOst = _.KoordinatOst,
            Dybde = _.Dybde,
            AntallGrabbhugg = _.Undersokelse?.AntallGrabbhugg ?? 0,
            Bunntype = _.Undersokelse?.BlotbunnId is null ? Bunntype.Hardbunn : Bunntype.Blotbunn,
        });
        
        report.FillPositions(copyResult.Value, positions);

        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.Position);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }
    
    public async Task<Response<string>> GenerateImagesReport(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(_ => _.BStasjons)
                .ThenInclude(_ => _.Undersokelse)
                    .ThenInclude(_ => _.BBilders)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.Image);
        if (copyResult.IsError)
        {
            return copyResult;
        }

        List<BStasjon> stations = dbRecord.BStasjons.OrderBy(_ => _.Nummer).ToList();
        List<RowImage> rows = [];
        
        foreach (BStasjon station in stations)
        {
            IEnumerable<BBilder> images = station.Undersokelse.BBilders;
            if (!images.Any())
            {
                rows.Add(new RowImage()
                {
                    Nummer = station.Nummer,
                    SiltImage = null,
                    UsiltImage = null,
                });
            }
            else
            {
                foreach (BBilder img in images)
                {
                    string path = Path.Combine(
                        webHostEnvironment.WebRootPath,
                        "images",
                        dbRecord.ProsjektIdSe,
                        $"{img.Id.ToString()}.{img.Extension}");
                    
                    using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    using MemoryStream memory = new MemoryStream();
                    
                    stream.CopyTo(memory);
                    byte[] content = memory.ToArray();

                    RowImage? rowImage = rows.FirstOrDefault(r => r.Nummer == station.Nummer);
                    if (rowImage is null)
                    {
                        rows.Add(new RowImage()
                        {
                            Nummer = station.Nummer,
                            SiltImage = img.Silt is true ? content : null,
                            UsiltImage = img.Silt is false ? content : null,
                        });
                    }
                    else
                    {
                        if (img.Silt)
                        {
                            rowImage.SiltImage = content;
                        }
                        else
                        {
                            rowImage.UsiltImage = content;
                        }
                    }
                }
            }
        }
        
        report.FillImages(copyResult.Value, rows);
        
        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.Image);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }

    public async Task<Response<string>> GeneratePtpReport(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(_ => _.Lokalitet)
            .Include(_ => _.Kunde)
            .Include(_ => _.BProvetakningsplan)
                .ThenInclude(_ => _.BStasjons)
            .Include(_ => _.BProvetakningsplan)
                .ThenInclude(_ => _.Planlegger)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.PTP);
        if (copyResult.IsError)
        {
            return copyResult;
        }

        PtpHeader header = new PtpHeader()
        {
            Oppdragsgiver = dbRecord.Kunde.Oppdragsgiver,
            Lokalitetsnavn = dbRecord.Lokalitet.Lokalitetsnavn,
            Planlagtfeltdato = dbRecord.BProvetakningsplan.Planlagtfeltdato,
            Planlegger = $"{dbRecord.BProvetakningsplan.Planlegger.Etternavn} {dbRecord.BProvetakningsplan.Planlegger.Fornavn}"
        };
        
        IEnumerable<RowPtp> rows = dbRecord.BProvetakningsplan.BStasjons
            .OrderBy(_ => _.Nummer)
            .Select(_ => new RowPtp()
        {
            Planlagtfeltdato = dbRecord.BProvetakningsplan.Planlagtfeltdato,
            Nummer = _.Nummer,
            KoordinatNord = _.KoordinatNord,
            KoordinatOst = _.KoordinatOst,
            Dybde = _.Dybde,
            Analyser = _.Analyser,
        });
        
        report.FillPtp(copyResult.Value, rows, header);
        
        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.PTP);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }

    public async Task<Response<string>> GeneratePlotreport(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(_ => _.BStasjons)
            .ThenInclude(_ => _.Undersokelse)
            .ThenInclude(_ => _.Sediment)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        Response<string> copyResult = report.CopyDocument(dbRecord.ProsjektIdSe, SheetName.Plot);
        if (copyResult.IsError)
        {
            return copyResult;
        }

        IEnumerable<PlotColumn> columns = dbRecord.BStasjons
            .OrderBy(_ => _.Nummer)
            .Select(_ => new PlotColumn()
        {
            Ph = _.Undersokelse?.SedimentId is null ? 0 : _.Undersokelse.Sediment.Ph,
            Eh = _.Undersokelse?.SedimentId is null ? 0 : _.Undersokelse.Sediment.Eh,
        });
        
        report.FillPhEhPlot(copyResult.Value, columns);
        
        Response saveResult = await CheckAndReplaceReport(projectId, SheetName.Plot);
        if (saveResult.IsError)
        {
            return Response<string>.Error(saveResult.ErrorMessage);
        }
        
        return Response<string>.Ok(copyResult.Value);
    }

    public async Task<Response<IEnumerable<Response<string>>>> GenerateAllReports(Guid projectId)
    {
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(_ => _.BUndersokelses)
            .ThenInclude(_ => _.Sediment)
            .Include(_ => _.BUndersokelses)
            .ThenInclude(_ => _.Sensorisk)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<IEnumerable<Response<string>>>.Error(ProjectNotFoundError);
        }
        
        // Calculate Sediments Tilstand
        List<BSediment?> sediments = dbRecord.BUndersokelses.Select(_ => _.Sediment).ToList();
        foreach (BSediment sediment in sediments)
        {
            if (sediment is not null)
            {
                Response response = tilstandService.CalculateSedimentTilstand(sediment);
                if (response.IsError)
                {
                    return Response<IEnumerable<Response<string>>>.Error(response.ErrorMessage);
                }
            }
        }
        
        Response sedimentUpdateResult = await sedimentRepository.UpdateRange(sediments.Where(_ => _ is not null));
        if (sedimentUpdateResult.IsError)
        {
            return Response<IEnumerable<Response<string>>>.Error(sedimentUpdateResult.ErrorMessage);
        }
        
        // Calculate Sensorisks Tilstand
        List<BSensorisk?> sensorisks = dbRecord.BUndersokelses.Select(_ => _.Sensorisk).ToList();
        foreach (BSensorisk sensorisk in sensorisks)
        {
            if (sensorisk is not null)
            {
                tilstandService.CalculateSensoriskTilstand(sensorisk);
            }
        }
        
        Response updateSensoriskResult = await sensoriskRepository.UpdateRange(sensorisks.Where(_ => _ is not null));
        if (updateSensoriskResult.IsError)
        {
            return Response<IEnumerable<Response<string>>>.Error(updateSensoriskResult.ErrorMessage);
        }
        
        // Calculate Undersøkelses Tilstand
        List<BUndersokelse> undersokelses = dbRecord.BUndersokelses.ToList();
        foreach (BUndersokelse undersokelse in undersokelses)
        {
            tilstandService.CalculateUndersokelseTilstand(undersokelse);
        }
        
        Response updateUndersokelseResult = await undersokelseRepository.UpdateRange(undersokelses);
        if (updateUndersokelseResult.IsError)
        {
            return Response<IEnumerable<Response<string>>>.Error(updateUndersokelseResult.ErrorMessage);
        }
        
        // Calculate Prosjekts Tilstand
        BTilstand? tilstand = await tilstandRepository.GetBy(_ => _.ProsjektId == dbRecord.Id);
        if (tilstand is not null)
        {
            Response removeResult = await tilstandRepository.Delete(tilstand);
            if (removeResult.IsError)
            {
                return Response<IEnumerable<Response<string>>>.Error(removeResult.ErrorMessage);
            }
        }
        
        tilstand = tilstandService.CalculateProsjektTilstand(dbRecord.BUndersokelses, dbRecord.Id);
        
        Response createTilstandResult = await tilstandRepository.Add(tilstand);
        if (createTilstandResult.IsError)
        {
            return Response<IEnumerable<Response<string>>>.Error(createTilstandResult.ErrorMessage);
        }
        
        return Response<IEnumerable<Response<string>>>.Ok([
            await GenerateInfoReport(projectId),
            await GeneratePositionsReport(projectId),
            await GenerateB1Report(projectId),
            await GenerateB2Report(projectId),
            await GenerateImagesReport(projectId),
            await GeneratePlotreport(projectId),
        ]);
    }

    public async Task<GetReportsDto> GetAllReports(Guid projectId)
    {
        List<BRapporter> dbRecords = await reportRepository.GetAll()
            .Where(_ => _.ProsjektId == projectId)
            .ToListAsync();
        
        return new GetReportsDto()
        {
            Reports = dbRecords.Where(_ => (SheetName)_.ArkNavn != SheetName.PTP).OrderBy(_ => _.ArkNavn).Select(MapReport)
        };
    }

    public async Task<Response<ReportDto>> GetPtpReport(Guid projectId)
    {
        BRapporter? dbRecord = await reportRepository.GetBy(_ =>
            _.ProsjektId == projectId &&
            _.ArkNavn == (int)SheetName.PTP);

        if (dbRecord is null)
        {
            return Response<ReportDto>.Error(ReportNotFoundError);
        }
        
        return Response<ReportDto>.Ok(MapReport(dbRecord));
    }
    
    public async Task<Response<FileModel>> DownloadReportById(Guid peportId)
    {
        BRapporter? dbRecord = await reportRepository.GetAll()
            .Include(_ => _.Prosjekt)
            .FirstOrDefaultAsync(_ => _.Id == peportId);

        if (dbRecord is null)
        {
            return Response<FileModel>.Error(ReportNotFoundError);
        }

        return report.DownloadReport(dbRecord.Prosjekt.ProsjektIdSe, (SheetName)dbRecord.ArkNavn);
    }
    
    private async Task<Response> CheckAndReplaceReport(Guid projectId, SheetName sheetName)
    {
        BRapporter? dbRecord = await reportRepository.GetBy(_ => _.ProsjektId == projectId && _.ArkNavn == (int)sheetName);
        if (dbRecord is null)
        {
            return await reportRepository.Add(new BRapporter()
            {
                Id = Guid.NewGuid(),
                Datogenerert = DateTime.Now,
                ArkNavn = (int)sheetName,
                ProsjektId = projectId,
            });
        }
        else
        {
            dbRecord.Datogenerert = DateTime.Now;
            
            return await reportRepository.Update(dbRecord);
        }
    }

    private ReportDto MapReport(BRapporter dbRecord) => new ReportDto()
    {
        Id = dbRecord.Id,
        SheetName = ((SheetName)dbRecord.ArkNavn).GetDescription(),
        DateCreated = dbRecord.Datogenerert
    };
}