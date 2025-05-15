using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaEco.Abstractions.Models.PreInfo;
using SeaEco.Server.Controllers;
using SeaEco.Services.PreInfo;

namespace SeaEco.ServerTests;

public class PreInfoControllerTests
{
    private readonly Mock<IPreInfoService> _svcMock;
    private readonly PreInfoController    _ctrl;

    public PreInfoControllerTests()
    {
        _svcMock = new Mock<IPreInfoService>();
        _ctrl    = new PreInfoController(_svcMock.Object);
    }

    [Fact]
    public async Task GetAllByProject_WhenListNonEmpty_ReturnsOkWithList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var list = new List<PreInfoDto>
        {
            new PreInfoDto { Id = Guid.NewGuid(), ProsjektId = projectId, Feltdato = DateTime.Today }
        };
        _svcMock.Setup(s => s.GetAllByProjectAsync(projectId))
                .ReturnsAsync(list);

        // Act
        var actionResult = await _ctrl.GetAllByProject(projectId);

        // Assert
        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.StatusCode.Should().Be(200);
        ((IEnumerable<PreInfoDto>)ok.Value!).Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task GetAllByProject_WhenEmpty_ReturnsNotFound()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _svcMock.Setup(s => s.GetAllByProjectAsync(projectId))
                .ReturnsAsync(new List<PreInfoDto>());

        // Act
        var actionResult = await _ctrl.GetAllByProject(projectId);

        // Assert
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsOkWithDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new PreInfoDto
        {
            Id               = id,
            ProsjektId       = Guid.NewGuid(),
            Feltdato         = DateTime.Today,
            FeltansvarligId  = Guid.NewGuid(),
            Ph               = 7.1f,
            Eh               = -100f,
            Temperatur       = 10f,
            RefElektrode     = 1,
            Grabb            = "1",
            Sil              = "1",
            PhMeter          = "1",
            Kalibreringsdato = DateOnly.FromDateTime(DateTime.Today)
        };
        _svcMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);

        // Act
        var actionResult = await _ctrl.GetById(id);

        // Assert
        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.StatusCode.Should().Be(200);
        ok.Value.Should().Be(dto);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _svcMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((PreInfoDto?)null);

        // Act
        var actionResult = await _ctrl.GetById(id);

        // Assert
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_WhenValidDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var dto   = new AddPreInfoDto
        {
            ProsjektId      = Guid.NewGuid(),
            Feltdato        = DateTime.Today,
            FeltansvarligId = Guid.NewGuid(),
            Ph              = 6.5f,
            Eh              = 0f,
            Temperatur      = 5f,
            RefElektrode    = 0,
            Grabb           = "2",
            Sil             = "1",
            PhMeter         = "1",
            Kalibreringsdato= DateOnly.FromDateTime(DateTime.Today)
        };
        var newId = Guid.NewGuid();
        _svcMock.Setup(s => s.CreatePreInfoAsync(dto))
                .ReturnsAsync(newId);

        // Act
        var actionResult = await _ctrl.Create(dto);

        // Assert
        var created = actionResult as CreatedAtActionResult;
        created.Should().NotBeNull();
        created!.ActionName.Should().Be(nameof(_ctrl.GetById));
        created.RouteValues!["preInfoId"].Should().Be(newId);
    }

    [Fact]
    public async Task Create_WhenDuplicate_ThrowsInvalidOperationException_ReturnsConflict()
    {
        // Arrange
        var dto = new AddPreInfoDto
        {
            ProsjektId      = Guid.NewGuid(),
            Feltdato        = DateTime.Today,
            FeltansvarligId = Guid.NewGuid(),
            Ph              = 6.5f,
            Eh              = 0f,
            Temperatur      = 5f,
            RefElektrode    = 0,
            Grabb           = "2",
            Sil             = "2",
            PhMeter         = "2",
            Kalibreringsdato= DateOnly.FromDateTime(DateTime.Today)
        };
        _svcMock.Setup(s => s.CreatePreInfoAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Duplicate preinfo"));

        // Act
        var actionResult = await _ctrl.Create(dto);

        // Assert
        var conflict = actionResult as ConflictObjectResult;
        conflict.Should().NotBeNull();
        conflict!.StatusCode.Should().Be(409);

        conflict.Value
            .Should().BeEquivalentTo(new { Message = "Duplicate preinfo" });
    }

    [Fact]
    public async Task Create_WhenProjectNotFound_ThrowsKeyNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var dto = new AddPreInfoDto
        {
            ProsjektId      = Guid.NewGuid(),
            Feltdato        = DateTime.Today,
            FeltansvarligId = Guid.NewGuid(),
            Ph              = 6.5f,
            Eh              = 0f,
            Temperatur      = 5f,
            RefElektrode    = 0,
            Grabb           = "2",
            Sil             = "2",
            PhMeter         = "2",
            Kalibreringsdato= DateOnly.FromDateTime(DateTime.Today)
        };
        _svcMock.Setup(s => s.CreatePreInfoAsync(dto))
                .ThrowsAsync(new KeyNotFoundException("Project not found"));

        // Act
        var actionResult = await _ctrl.Create(dto);

        // Assert
        var notFound = actionResult as NotFoundObjectResult;
        notFound.Should().NotBeNull();
        notFound!.StatusCode.Should().Be(404);
        notFound.Value.Should().Be("Project not found");
    }

    [Fact]
    public async Task Delete_WhenExists_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _svcMock.Setup(s => s.DeletePreInfoAsync(id)).ReturnsAsync(true);

        // Act
        var actionResult = await _ctrl.Delete(id);

        // Assert
        actionResult.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _svcMock.Setup(s => s.DeletePreInfoAsync(id)).ReturnsAsync(false);

        // Act
        var actionResult = await _ctrl.Delete(id);

        // Assert
        actionResult.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Edit_WhenValid_ReturnsOkWithUpdatedDto()
    {
        // Arrange
        var id  = Guid.NewGuid();
        var dto = new EditPreInfoDto
        {
            Feltdato        = DateTime.Today,
            FeltansvarligId = Guid.NewGuid(),
            Ph              = 7.0f,
            Eh              = 0f,
            Temperatur      = 5f,
            RefElektrode    = 1,
            Grabb           = "1",
            Sil             = "2",
            PhMeter         = "1",
            Kalibreringsdato= DateOnly.FromDateTime(DateTime.Today)
        };
        var updatedDto = new PreInfoDto
        {
            Id               = id,
            ProsjektId       = Guid.NewGuid(),
            Feltdato         = dto.Feltdato,
            FeltansvarligId  = dto.FeltansvarligId,
            ProvetakerIds    = new List<Guid>(),
            Ph               = dto.Ph,
            Eh               = dto.Eh,
            Temperatur       = dto.Temperatur,
            RefElektrode     = dto.RefElektrode,
            Grabb            = dto.Grabb,
            Sil              = dto.Sil,
            PhMeter          = dto.PhMeter,
            Kalibreringsdato = dto.Kalibreringsdato
        };
        _svcMock.Setup(s => s.EditPreInfoAsync(id, dto)).ReturnsAsync(updatedDto);

        // Act
        var actionResult = await _ctrl.Edit(id, dto);

        // Assert
        var ok = actionResult as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);
        ok.Value.Should().Be(updatedDto);
    }

    [Fact]
    public async Task Edit_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        var id  = Guid.NewGuid();
        var dto = new EditPreInfoDto
        {
            Feltdato        = DateTime.Today,
            FeltansvarligId = Guid.NewGuid(),
            Ph              = 7.0f,
            Eh              = 0f,
            Temperatur      = 5f,
            RefElektrode    = 1,
            Grabb           = "1",
            Sil             = "1",
            PhMeter         = "1",
            Kalibreringsdato= DateOnly.FromDateTime(DateTime.Today)
        };
        _svcMock.Setup(s => s.EditPreInfoAsync(id, dto)).ReturnsAsync((PreInfoDto?)null);

        // Act
        var actionResult = await _ctrl.Edit(id, dto);

        // Assert
        actionResult.Should().BeOfType<NotFoundResult>();
    }
}