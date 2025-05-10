using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Enums.Bsensorisk;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Abstractions.ValueObjects.Bunnsubstrat;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Reporter;
using SeaEco.Reporter.Models;
using SeaEco.Reporter.Models.B1;
using SeaEco.Reporter.Models.B2;
using SeaEco.Reporter.Models.Info;
using SeaEco.Reporter.Models.Positions;

namespace SeaEco.Services.ReportServices;

public sealed class ReportService(Report report, IGenericRepository<BProsjekt> projectRepository) : IReportService
{
    private const string ProjectNotFoundError = "Project not found";

    public async Task<Response<string>> GenerateInfoReport(Guid projectId)
    {
        Response<string> copyResult = report.CopyDocument(SheetName.Info);
        if (copyResult.IsError)
        {
            return copyResult;
        }

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
        
        IEnumerable<BUndersokelse> undersokelses = dbRecord.BUndersokelses;

        report.FillInfo(copyResult.Value, new CommonInformation()
        {
            ProsjektIdSe = dbRecord.ProsjektIdSe ?? string.Empty,
            FeltDatoer = dbRecord.BPreinfos.Select(_ => _.Feltdato),

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

        return Response<string>.Ok(copyResult.Value);
    }

    public async Task<Response<string>> GenerateB1Report(Guid projectId)
    {
        Response<string> copyResult = report.CopyDocument(SheetName.B1);
        if (copyResult.IsError)
        {
            return copyResult;
        }
        
        BProsjekt? dbRecord = await projectRepository.GetAll()
            .Include(project => project.Kunde)
            .Include(project => project.BPreinfos)
            .Include(project => project.Lokalitet)
            .Include(project => project.BTilstand)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Sediment)
            .Include(project => project.BUndersokelses)
                .ThenInclude(undersokelses => undersokelses.Sensorisk)
            .FirstOrDefaultAsync(_ => _.Id == projectId);

        if (dbRecord is null)
        {
            return Response<string>.Error(ProjectNotFoundError);
        }
        
        IEnumerable<ColumnB1> columns = dbRecord.BUndersokelses.Select(_ => new ColumnB1()
        {
            Bunntype = _.HardbunnId is null ? Bunntype.Blotbunn : Bunntype.Hardbunn,
            Dyr = _.DyrId is null ? Dyr.Nei : Dyr.Ja,
            
            pH = _.Sediment?.Ph ?? 0,
            Eh = _.Sediment?.Eh ?? 0,
            phEh = _.Sediment?.KlasseGr2 ?? 0,
            TilstandProveGr2 = (Tilstand)(_.Sediment?.TilstandGr2 ?? 0),
            
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
            TilstandProveGr3 = (Tilstand)(_.Sensorisk?.TilstandGr3 ?? 0),
            MiddelVerdiGr2Gr3 = _.IndeksGr2Gr3 ?? 0,
            TilstandProveGr2Gr3 = (Tilstand)(_.TilstandGr2Gr3 ?? 0)
        });

        BHeader header = new BHeader()
        {
            Oppdragsgiver = dbRecord.Kunde.Oppdragsgiver,
            FeltDatoer = dbRecord.BPreinfos.Select(_ => _.Feltdato),
            Lokalitetsnavn = dbRecord.Lokalitet.Lokalitetsnavn,
            LokalitetsID = dbRecord.Lokalitet.LokalitetsId
        };
        
        BPreinfo? dbPreinfo = dbRecord.BPreinfos.OrderBy(_ => _.Feltdato).FirstOrDefault();
        
        SjovannB1 sjovann = new SjovannB1()
        {
            SjoTemperatur = dbPreinfo?.SjoTemperatur ?? 0,
            pHSjo = dbPreinfo?.PhSjo ?? 0,
            EhSjo = dbPreinfo?.EhSjo ?? 0,
            SedimentTemperatur = dbRecord.BUndersokelses.Any() ? dbRecord.BUndersokelses.Average(u => u.Sediment?.Temperatur ?? 0): 0,
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
        
        Response<string> copyResult = report.CopyDocument(SheetName.B2);
        if (copyResult.IsError)
        {
            return copyResult;
        }

        IEnumerable<ColumnB2> columns = dbRecord.BUndersokelses.Select(_ => new ColumnB2()
        {
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
            FeltDatoer = dbRecord.BPreinfos.Select(_ => _.Feltdato),
            Lokalitetsnavn = dbRecord.Lokalitet.Lokalitetsnavn,
            LokalitetsID = dbRecord.Lokalitet.LokalitetsId
        };
        
        report.FillB2(copyResult.Value, columns, header);

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
        
        Response<string> copyResult = report.CopyDocument(SheetName.Position);
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

        return Response<string>.Ok(copyResult.Value);
    }
}