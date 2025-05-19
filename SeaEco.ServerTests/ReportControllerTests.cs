using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Models.Report;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.Reporter.Models;
using SeaEco.Services.ReportServices;
using SeaEco.Services.TilstandServices;


namespace SeaEco.ServerTests;

public class ReportControllerTests : IDisposable
{
    private readonly Mock<IReportService> _reportServiceMock;
    private readonly TilstandService _tilstandService;
    private readonly AppDbContext _db;
    private readonly SeaEco.Server.Controllers.ReportController _ctrl;

    public ReportControllerTests()
    {
        _reportServiceMock = new Mock<IReportService>();
        _tilstandService   = new TilstandService();

        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new TestDbContext(opts);
        SeedDb();

        _ctrl = new SeaEco.Server.Controllers.ReportController(
            _reportServiceMock.Object,
            _tilstandService,
            _db);
    }

    private void SeedDb()
    {
        var kunde = new Kunde {
            Id = Guid.NewGuid(), Oppdragsgiver = "Test AS",
            Kontaktperson = "Ola", Telefon = "12345678"
        };
        _db.Kundes.Add(kunde);

        var lokalitet = new Lokalitet {
            Id = Guid.NewGuid(), Lokalitetsnavn = "Testvik", LokalitetsId = "00001"
        };
        _db.Lokalitets.Add(lokalitet);

        var bruker = new Bruker {
            Id = Guid.NewGuid(), Fornavn = "Test", Etternavn = "Ansvarlig",
            Epost = "t@a.no", PassordHash = "h", Salt = Array.Empty<byte>(),
            IsAdmin = false, Aktiv = true
        };
        _db.Brukers.Add(bruker);

        var prosjekt = new BProsjekt {
            Id = Guid.NewGuid(), PoId = "PO1234",
            KundeId = kunde.Id, Kundekontaktperson = "Ola",
            Kundetlf = "87654321", LokalitetId = lokalitet.Id,
            ProsjektansvarligId = bruker.Id,
            Produksjonsstatus = 1, Prosjektstatus = 1,
            Datoregistrert = DateTime.UtcNow
        };
        _db.BProsjekts.Add(prosjekt);

        _db.BPreinfos.Add(new BPreinfo {
            Id = Guid.NewGuid(), ProsjektId = prosjekt.Id,
            Feltdato = DateTime.UtcNow, FeltansvarligId = bruker.Id,
            PhSjo = 8f, EhSjo = 100f, SjoTemperatur = 12f,
            RefElektrode = 1, Grabb = "G", Sil = "S",
            PhMeter = "PM", Kalibreringsdato = DateOnly.FromDateTime(DateTime.UtcNow)
        });

        _db.BTilstands.Add(new BTilstand {
            Id = Guid.NewGuid(), ProsjektId = prosjekt.Id,
            IndeksGr2 = 1.5f, TilstandGr2 = 2,
            IndeksGr3 = 2.5f, TilstandGr3 = 3,
            IndeksLokalitet = 2f, TilstandLokalitet = 2
        });

        var sediment = new BSediment {
            Id = Guid.NewGuid(), Ph = 7.5f, Eh = 80f, Temperatur = 10f
        };
        _db.BSediments.Add(sediment);

        var sensorisk = new BSensorisk {
            Id = Guid.NewGuid(),
            Gassbobler = 1, Farge = 1, Lukt = 1,
            Konsistens = 1, Grabbvolum = 1, Tykkelseslamlag = 1
        };
        _db.BSensorisks.Add(sensorisk);

        var undersokelse = new BUndersokelse {
            Id = Guid.NewGuid(),
            ProsjektId = prosjekt.Id,
            PreinfoId = _db.BPreinfos.Local.First().Id,
            Feltdato = DateOnly.FromDateTime(DateTime.Today),
            AntallGrabbhugg = 5,
            GrabbhastighetGodkjent = true,
            SedimentId = sediment.Id,
            SensoriskId = sensorisk.Id,
            DatoRegistrert = DateTime.UtcNow
        };
        _db.BUndersokelses.Add(undersokelse);

        _db.BStasjons.Add(new BStasjon {
            Id = Guid.NewGuid(),
            ProsjektId = prosjekt.Id,
            Nummer = 1,
            KoordinatNord = "59N",
            KoordinatOst = "10E",
            Dybde = 20,
            Analyser = "A",
            UndersokelseId = undersokelse.Id
        });

        _db.BRapporters.Add(new BRapporter {
            Id = Guid.NewGuid(),
            ProsjektId = prosjekt.Id,
            ArkNavn = (int)SheetName.Info,
            Datogenerert = DateTime.UtcNow
        });

        _db.SaveChanges();
    }

    public void Dispose() => _db.Dispose();


    //
    // 1) Eksplisitte Generate*-tester
    //

    [Fact]
    public async Task GenerateInfo_OnSuccess_ReturnsOkWithFilename()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateInfoReport(projectId))
            .ReturnsAsync(Response<string>.Ok("info.xlsx"));

        var action = await _ctrl.GenerateInfo(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be("info.xlsx");
    }

    [Fact]
    public async Task GenerateInfo_OnError_ReturnsBadRequest()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateInfoReport(projectId))
            .ReturnsAsync(Response<string>.Error("fail-info"));

        var action = await _ctrl.GenerateInfo(projectId);

        var bad = Assert.IsType<BadRequestObjectResult>(action);
        bad.Value.Should().BeEquivalentTo(new { errorMessage = "fail-info" });
    }

    [Fact]
    public async Task GenerateB1_OnSuccess_ReturnsOkWithFilename()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateB1Report(projectId))
            .ReturnsAsync(Response<string>.Ok("b1.xlsx"));

        var action = await _ctrl.GenerateB1(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be("b1.xlsx");
    }

    [Fact]
    public async Task GenerateB1_OnError_ReturnsBadRequest()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateB1Report(projectId))
            .ReturnsAsync(Response<string>.Error("fail-b1"));

        var action = await _ctrl.GenerateB1(projectId);

        var bad = Assert.IsType<BadRequestObjectResult>(action);
        bad.Value.Should().BeEquivalentTo(new { errorMessage = "fail-b1" });
    }

    [Fact]
    public async Task GenerateB2_OnSuccess_ReturnsOkWithFilename()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateB2Report(projectId))
            .ReturnsAsync(Response<string>.Ok("b2.xlsx"));

        var action = await _ctrl.GenerateB2(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be("b2.xlsx");
    }

    [Fact]
    public async Task GeneratePositions_OnSuccess_ReturnsOkWithFilename()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GeneratePositionsReport(projectId))
            .ReturnsAsync(Response<string>.Ok("pos.xlsx"));

        var action = await _ctrl.GeneratePositions(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be("pos.xlsx");
    }

    [Fact]
    public async Task GenerateImages_OnSuccess_ReturnsOkWithFilename()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateImagesReport(projectId))
            .ReturnsAsync(Response<string>.Ok("img.xlsx"));

        var action = await _ctrl.GenerateImages(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be("img.xlsx");
    }

    [Fact]
    public async Task GeneratePtp_OnSuccess_ReturnsOkWithFilename()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GeneratePtpReport(projectId))
            .ReturnsAsync(Response<string>.Ok("ptp.xlsx"));

        var action = await _ctrl.GeneratePtp(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be("ptp.xlsx");
    }


    //
    // 2) GenerateAll
    //

    [Fact]
    public async Task GenerateAll_OnSuccess_ReturnsOkWithDto()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateAllReports(projectId))
            .ReturnsAsync(Response<IEnumerable<Response<string>>>.Ok(new[]{
                Response<string>.Ok("a"), Response<string>.Ok("b")
            }));
        var dto = new GetReportsDto { Reports = Array.Empty<ReportDto>() };
        _reportServiceMock
            .Setup(s => s.GetAllReports(projectId))
            .ReturnsAsync(dto);

        var action = await _ctrl.GenerateAll(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be(dto);
    }

    [Fact]
    public async Task GenerateAll_OnError_ReturnsBadRequest()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GenerateAllReports(projectId))
            .ReturnsAsync(Response<IEnumerable<Response<string>>>.Error("nope"));

        var action = await _ctrl.GenerateAll(projectId);

        var bad = Assert.IsType<BadRequestObjectResult>(action);
        bad.Value.Should().BeEquivalentTo(new { errorMessage = "nope" });
    }


    //
    // 3) GetAll, GetPtp, Download
    //

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var projectId = Guid.NewGuid();
        var dto = new GetReportsDto { Reports = Array.Empty<ReportDto>() };
        _reportServiceMock
            .Setup(s => s.GetAllReports(projectId))
            .ReturnsAsync(dto);

        var action = await _ctrl.GetAll(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be(dto);
    }

    [Fact]
    public async Task GetPtp_OnSuccess_ReturnsOk()
    {
        var projectId = Guid.NewGuid();
        var rpt = new ReportDto { Id = Guid.NewGuid(), SheetName = "PTP", DateCreated = DateTime.UtcNow };
        _reportServiceMock
            .Setup(s => s.GetPtpReport(projectId))
            .ReturnsAsync(Response<ReportDto>.Ok(rpt));

        var action = await _ctrl.GetPtp(projectId);

        var ok = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().Be(rpt);
    }

    [Fact]
    public async Task GetPtp_OnError_ReturnsBadRequest()
    {
        var projectId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.GetPtpReport(projectId))
            .ReturnsAsync(Response<ReportDto>.Error("missing"));

        var action = await _ctrl.GetPtp(projectId);

        var bad = Assert.IsType<BadRequestObjectResult>(action);
        bad.Value.Should().BeEquivalentTo(new { errorMessage = "missing" });
    }

    [Fact]
    public async Task DownloadReport_OnSuccess_ReturnsFile()
    {
        var reportId = Guid.NewGuid();
        var model = new FileModel {
            Content = new byte[] {1,2,3},
            ContentType = "application/pdf",
            DownloadName = "foo.pdf"
        };
        _reportServiceMock
            .Setup(s => s.DownloadReportById(reportId))
            .ReturnsAsync(Response<FileModel>.Ok(model));

        var action = await _ctrl.DownloadReport(reportId);

        var file = Assert.IsType<FileContentResult>(action);
        file.FileContents.Should().Equal(model.Content);
        file.ContentType.Should().Be(model.ContentType);
        file.FileDownloadName.Should().Be(model.DownloadName);
    }

    [Fact]
    public async Task DownloadReport_OnError_ReturnsBadRequest()
    {
        var reportId = Guid.NewGuid();
        _reportServiceMock
            .Setup(s => s.DownloadReportById(reportId))
            .ReturnsAsync(Response<FileModel>.Error("nf"));

        var action = await _ctrl.DownloadReport(reportId);

        var bad = Assert.IsType<BadRequestObjectResult>(action);
        bad.Value.Should().BeEquivalentTo(new { errorMessage = "nf" });
    }


    //
    // 4) Tilstand‐endpoints
    //

    [Fact]
    public void GenerateClass_ValidPHEh_ReturnsOkWithResponseInt()
    {
        var model = new SeaEco.Services.TilstandServices.Models.CalculateClassModel { pH = 8.5, eH = 200 };
        var action = _ctrl.GenerateClass(model).Result;

        var ok = Assert.IsType<OkObjectResult>(action);
        var resp = Assert.IsType<Response<int>>(ok.Value);
        resp.Value.Should().Be(0);  
    }

    [Fact]
    public async Task CalculateSediment_NotFound_ReturnsNotFound()
    {
        var action = await _ctrl.CalculateSediment(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(action);
    }

    [Fact]
    public async Task CalculateSediment_Found_ReturnsOkAndUpdates()
    {
        var sed = _db.BSediments.First();
        var action = await _ctrl.CalculateSediment(sed.Id);
        var ok = Assert.IsType<OkObjectResult>(action);
        var updated = Assert.IsType<BSediment>(ok.Value);
        updated.TilstandGr2.Should().NotBeNull();
    }

    [Fact]
    public async Task CalculateSensorisk_NotFound_ReturnsNotFound()
    {
        var action = await _ctrl.CalculateSensorisk(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(action);
    }

    [Fact]
    public async Task CalculateSensorisk_Found_ReturnsOkAndUpdates()
    {
        var sensor = _db.BSensorisks.First();
        var action = await _ctrl.CalculateSensorisk(sensor.Id);
        var ok = Assert.IsType<OkObjectResult>(action);
        var updated = Assert.IsType<BSensorisk>(ok.Value);
        updated.IndeksGr3.Should().NotBeNull();
    }

    [Fact]
    public async Task CalculateUndersokelse_NotFound_ReturnsNotFound()
    {
        var action = await _ctrl.CalculateUndersokelseTilstand(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(action);
    }

    [Fact]
    public async Task CalculateUndersokelse_Found_ReturnsOkAndUpdates()
    {
        var survey = _db.BUndersokelses
                        .Include(u => u.Sediment)
                        .Include(u => u.Sensorisk)
                        .First();
        var action = await _ctrl.CalculateUndersokelseTilstand(survey.Id);
        var ok     = Assert.IsType<OkObjectResult>(action);
        var updated= Assert.IsType<BUndersokelse>(ok.Value);
        updated.TilstandGr2Gr3.Should().NotBeNull();
    }

    [Fact]
    public async Task CalculateProject_NotFound_ReturnsNotFound()
    {
        var action = await _ctrl.CalculateProject(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(action);
    }

    [Fact]
    public async Task CalculateProject_Found_ReturnsOkWithBTilstand()
    {
        var prosjektId = _db.BProsjekts.Select(p => p.Id).First();
        var action     = await _ctrl.CalculateProject(prosjektId);
        var ok         = Assert.IsType<OkObjectResult>(action);
        ok.Value.Should().BeOfType<BTilstand>();
    }
}