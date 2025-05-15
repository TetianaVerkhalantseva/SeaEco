using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Project;
using SeaEco.Abstractions.Models.SamplingPlan;
using SeaEco.Abstractions.Models.Stations;
using SeaEco.Server.Controllers;
using SeaEco.Services.BSurveyService;
using SeaEco.Services.ProjectServices;
using SeaEco.Services.SamplingPlanServices;
using SeaEco.Services.StationServices;

namespace SeaEco.ServerTests;

public class ProjectControllerTests
    {
        private readonly Mock<IProjectService>        _projectMock;
        private readonly Mock<ISamplingPlanService>   _samplingPlanMock;
        private readonly Mock<IStationService>        _stationMock;
        private readonly Mock<IBSurveyService>        _surveyMock;
        private readonly ProjectController            _ctrl;

        public ProjectControllerTests()
        {
            _projectMock      = new Mock<IProjectService>();
            _samplingPlanMock = new Mock<ISamplingPlanService>();
            _stationMock      = new Mock<IStationService>();
            _surveyMock       = new Mock<IBSurveyService>();

            _ctrl = new ProjectController(
                _projectMock.Object,
                _stationMock.Object,
                _samplingPlanMock.Object,
                _surveyMock.Object
            );

            // For å teste ModelState-validering
            _ctrl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        #region CreateProject

        [Fact]
        public async Task CreateProject_ValidDto_ReturnsOkWithId()
        {
            // Arrange
            var dto   = new NewProjectDto
            {
                PoId                = "123",
                KundeId             = Guid.NewGuid(),
                Kundekontaktperson  = "Ola",
                Kundeepost          = "ola@n.no",
                Lokalitetsnavn      = "Test",
                LokalitetsId        = "00001",
                Mtbtillatelse       = 1,
                ProsjektansvarligId = Guid.NewGuid(),
                Produksjonsstatus   = SeaEco.Abstractions.Enums.Produksjonsstatus.HalvBelastning
            };
            var newId = Guid.NewGuid();
            _projectMock
                .Setup(s => s.CreateProjectAsync(dto))
                .ReturnsAsync(newId);

            // Act
            var result = await _ctrl.CreateProject(dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(new { Id = newId });
        }

        [Fact]
        public async Task CreateProject_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("PoId", "Påkrevd");
            var dto = new NewProjectDto();

            // Act
            var result = await _ctrl.CreateProject(dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task CreateProject_ServiceThrowsKeyNotFound_ReturnsNotFound()
        {
            // Arrange
            var dto = new NewProjectDto
            {
                PoId                = "123",
                KundeId             = Guid.NewGuid(),
                Kundekontaktperson  = "Ola",
                Kundeepost          = "ola@n.no",
                Lokalitetsnavn      = "Test",
                LokalitetsId        = "00001",
                Mtbtillatelse       = 1,
                ProsjektansvarligId = Guid.NewGuid(),
                Produksjonsstatus   = SeaEco.Abstractions.Enums.Produksjonsstatus.ForUtsett
            };
            _projectMock
                .Setup(s => s.CreateProjectAsync(dto))
                .ThrowsAsync(new KeyNotFoundException("Prosjekt ikke funnet"));

            // Act
            var result = await _ctrl.CreateProject(dto);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Prosjekt ikke funnet");
        }

        [Fact]
        public async Task CreateProject_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var dto = new NewProjectDto
            {
                PoId                = "123",
                KundeId             = Guid.NewGuid(),
                Kundekontaktperson  = "Ola",
                Kundeepost          = "ola@n.no",
                Lokalitetsnavn      = "Test",
                LokalitetsId        = "00001",
                Mtbtillatelse       = 1,
                ProsjektansvarligId = Guid.NewGuid(),
                Produksjonsstatus   = Abstractions.Enums.Produksjonsstatus.MaksBelastning
            };
            _projectMock
                .Setup(s => s.CreateProjectAsync(dto))
                .ThrowsAsync(new Exception("Uventet feil"));

            // Act
            var result = await _ctrl.CreateProject(dto);

            // Assert
            var obj = result as ObjectResult;
            obj.Should().NotBeNull();
            obj!.StatusCode.Should().Be(500);
            obj.Value.Should().Be("Uventet feil");
        }

        #endregion

        #region GetAllProjects

        [Fact]
        public async Task GetAllProjects_ReturnsOkWithList()
        {
            // Arrange
            var list = new List<ProjectDto>
            {
                new ProjectDto { Id = Guid.NewGuid(), PoId = "1" },
                new ProjectDto { Id = Guid.NewGuid(), PoId = "2" }
            };
            _projectMock
                .Setup(s => s.GetAllProjectsAsync())
                .ReturnsAsync(list);

            // Act
            var result = await _ctrl.GetAllProjects();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(list);
        }

        #endregion

        #region GetAllProjectsByCustomer

        [Fact]
        public async Task GetAllProjectsByCustomerId_ReturnsOkWithList()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var list = new List<ProjectDto>
            {
                new ProjectDto { Id = Guid.NewGuid(), PoId = "X" }
            };
            _projectMock
                .Setup(s => s.GetAllProjectsByCustomerId(customerId))
                .ReturnsAsync(list);

            // Act
            var result = await _ctrl.GetAllProjectsByCustomerId(customerId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(list);
        }

        #endregion

        #region GetProjectById

        [Fact]
        public async Task GetProjectById_Found_ReturnsOkWithDto()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new ProjectDto { Id = id, PoId = "42" };
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(id))
                .ReturnsAsync(dto);

            // Act
            var result = await _ctrl.GetProjectById(id);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(dto);
        }

        [Fact]
        public async Task GetProjectById_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(id))
                .ReturnsAsync((ProjectDto?)null);

            // Act
            var result = await _ctrl.GetProjectById(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region UpdateProject

        [Fact]
        public async Task UpdateProject_ValidDto_ReturnsOkWithUpdatedDto()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditProjectDto { PoId = "99" };
            var updated = new ProjectDto { Id = id, PoId = "99" };

            _projectMock
                .Setup(s => s.UpdateProjectAsync(id, dto))
                .ReturnsAsync(updated);

            // Act
            var result = await _ctrl.UpdateProject(id, dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(updated);
        }

        [Fact]
        public async Task UpdateProject_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("PoId", "Feil");
            var dto = new EditProjectDto();

            // Act
            var result = await _ctrl.UpdateProject(Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateProject_ServiceThrowsKeyNotFound_ReturnsNotFound()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditProjectDto { PoId = "99" };
            _projectMock
                .Setup(s => s.UpdateProjectAsync(id, dto))
                .ThrowsAsync(new KeyNotFoundException("Ikke funnet"));

            // Act
            var result = await _ctrl.UpdateProject(id, dto);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Ikke funnet");
        }

        [Fact]
        public async Task UpdateProject_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditProjectDto { PoId = "99" };
            _projectMock
                .Setup(s => s.UpdateProjectAsync(id, dto))
                .ThrowsAsync(new Exception("Serverfeil"));

            // Act
            var result = await _ctrl.UpdateProject(id, dto);

            // Assert
            var obj = result as ObjectResult;
            obj.Should().NotBeNull();
            obj!.StatusCode.Should().Be(500);
            obj.Value.Should().Be("Serverfeil");
        }

        #endregion

        #region AddMerknad

        [Fact]
        public async Task AddMerknad_Valid_ReturnsOk()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto       = new MerknadDto { Merknad = "Test" };

            // Act
            var result = await _ctrl.AddMerknad(projectId, dto);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task AddMerknad_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Merknad", "Påkrevd");

            // Act
            var result = await _ctrl.AddMerknad(Guid.NewGuid(), new MerknadDto());

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task AddMerknad_ServiceThrowsKeyNotFound_ReturnsNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto       = new MerknadDto { Merknad = "X" };
            _projectMock
                .Setup(s => s.AddMerknadAsync(projectId, "X"))
                .ThrowsAsync(new KeyNotFoundException("Ingen prosjekt"));

            // Act
            var result = await _ctrl.AddMerknad(projectId, dto);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Ingen prosjekt");
        }

        #endregion

        #region UpdateStatus

        [Fact]
        public async Task UpdateStatus_Valid_ReturnsOk()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto       = new UpdateStatusDto { Status = SeaEco.Abstractions.Enums.Prosjektstatus.Nytt };

            // Act
            var result = await _ctrl.UpdateStatus(projectId, dto);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateStatus_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Status", "Feil");

            // Act
            var result = await _ctrl.UpdateStatus(Guid.NewGuid(), new UpdateStatusDto());

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateStatus_ServiceThrowsKeyNotFound_ReturnsNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto = new UpdateStatusDto { Status = Abstractions.Enums.Prosjektstatus.Nytt };
            _projectMock
                .Setup(s => s.UpdateProjectStatusAsync(projectId, dto.Status, dto.Merknad))
                .ThrowsAsync(new KeyNotFoundException("Ikke funnet"));

            // Act
            var result = await _ctrl.UpdateStatus(projectId, dto);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Ikke funnet");
        }

        #endregion

        #region SamplingPlan Operations

        [Fact]
        public async Task GetProjectSamplingPlan_Found_ReturnsOk()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var sp = new SamplingPlanDto
            {
                Id               = Guid.NewGuid(),
                ProsjektId       = projectId,
                Planlagtfeltdato = DateOnly.FromDateTime(DateTime.Today),
                PlanleggerId     = Guid.NewGuid()
            };
            _samplingPlanMock
                .Setup(s => s.GetSamplingPlanById(projectId))
                .ReturnsAsync(sp);

            // Act
            var result = await _ctrl.GetProjectSamplingPlan(projectId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(sp);
        }

        [Fact]
        public async Task GetProjectSamplingPlan_NotFound_ReturnsNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _samplingPlanMock
                .Setup(s => s.GetSamplingPlanById(projectId))
                .ReturnsAsync((SamplingPlanDto?)null);

            // Act
            var result = await _ctrl.GetProjectSamplingPlan(projectId);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be($"No sampling plan found with id {projectId}");
        }

        [Fact]
        public async Task GetProjectSamplingPlan_WrongProject_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var otherId   = Guid.NewGuid();
            var sp = new SamplingPlanDto
            {
                Id               = Guid.NewGuid(),
                ProsjektId       = otherId,
                Planlagtfeltdato = DateOnly.FromDateTime(DateTime.Today),
                PlanleggerId     = Guid.NewGuid()
            };
            _samplingPlanMock
                .Setup(s => s.GetSamplingPlanById(projectId))
                .ReturnsAsync(sp);

            // Act
            var result = await _ctrl.GetProjectSamplingPlan(projectId);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Sampling plan does not belong to the given project");
        }

        [Fact]
        public async Task CreateSamplingPlan_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("ProsjektId", "Feil");
            var dto = new EditSamplingPlanDto();

            // Act
            var result = await _ctrl.CreateSamplingPlan(Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task CreateSamplingPlan_ServiceError_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto = new EditSamplingPlanDto
            {
                Planlagtfeltdato = DateOnly.FromDateTime(DateTime.Today),
                PlanleggerId     = Guid.NewGuid()
            };
            _samplingPlanMock
                .Setup(s => s.CreateSamplingPlan(dto))
                .ReturnsAsync(new EditSamplingPlanResult { IsSuccess = false, Message = "Feil" });

            // Act
            var result = await _ctrl.CreateSamplingPlan(projectId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        [Fact]
        public async Task CreateSamplingPlan_Success_ReturnsCreatedAtAction()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto = new EditSamplingPlanDto
            {
                ProsjektId       = projectId,
                Planlagtfeltdato = DateOnly.FromDateTime(DateTime.Today),
                PlanleggerId     = Guid.NewGuid()
            };
            var sp = new SamplingPlanDto
            {
                Id               = Guid.NewGuid(),
                ProsjektId       = projectId,
                Planlagtfeltdato = dto.Planlagtfeltdato,
                PlanleggerId     = dto.PlanleggerId
            };
            _samplingPlanMock
                .Setup(s => s.CreateSamplingPlan(dto))
                .ReturnsAsync(new EditSamplingPlanResult { IsSuccess = true });
            _samplingPlanMock
                .Setup(s => s.GetSamplingPlanById(projectId))
                .ReturnsAsync(sp);

            // Act
            var result = await _ctrl.CreateSamplingPlan(projectId, dto);

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.ActionName.Should().Be(nameof(_ctrl.GetProjectSamplingPlan));
            created.RouteValues!["projectId"].Should().Be(projectId);
            created.Value.Should().Be(sp);
        }

        [Fact]
        public async Task UpdateSamplingPlan_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Planleggingsdato", "Feil");
            var dto = new EditSamplingPlanDto();

            // Act
            var result = await _ctrl.UpdateSamplingPlan(Guid.NewGuid(), Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateSamplingPlan_Success_ReturnsOkWithMessage()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var dto    = new EditSamplingPlanDto
            {
                ProsjektId       = Guid.NewGuid(),
                Planlagtfeltdato = DateOnly.FromDateTime(DateTime.Today),
                PlanleggerId     = Guid.NewGuid()
            };
            _samplingPlanMock
                .Setup(s => s.UpdateSamplingPlan(planId, dto))
                .ReturnsAsync(new EditSamplingPlanResult { IsSuccess = true, Message = "OK" });

            // Act
            var result = await _ctrl.UpdateSamplingPlan(dto.ProsjektId, planId, dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("OK");
        }

        [Fact]
        public async Task UpdateSamplingPlan_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var dto    = new EditSamplingPlanDto
            {
                ProsjektId       = Guid.NewGuid(),
                Planlagtfeltdato = DateOnly.FromDateTime(DateTime.Today),
                PlanleggerId     = Guid.NewGuid()
            };
            _samplingPlanMock
                .Setup(s => s.UpdateSamplingPlan(planId, dto))
                .ReturnsAsync(new EditSamplingPlanResult { IsSuccess = false, Message = "Feil" });

            // Act
            var result = await _ctrl.UpdateSamplingPlan(dto.ProsjektId, planId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        [Fact]
        public async Task DeleteSamplingPlan_Success_ReturnsOkWithMessage()
        {
            // Arrange
            var planId = Guid.NewGuid();
            _samplingPlanMock
                .Setup(s => s.DeleteSamplingPlan(It.IsAny<Guid>(), planId))
                .ReturnsAsync(new EditSamplingPlanResult { IsSuccess = true, Message = "Slettet" });

            // Act
            var result = await _ctrl.DeleteSamplingPlan(Guid.NewGuid(), planId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("Slettet");
        }

        [Fact]
        public async Task DeleteSamplingPlan_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var planId = Guid.NewGuid();
            _samplingPlanMock
                .Setup(s => s.DeleteSamplingPlan(It.IsAny<Guid>(), planId))
                .ReturnsAsync(new EditSamplingPlanResult { IsSuccess = false, Message = "Feil" });

            // Act
            var result = await _ctrl.DeleteSamplingPlan(Guid.NewGuid(), planId);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        #endregion

        #region Station Operations

        [Fact]
        public async Task GetAllStations_Success_ReturnsOkWithStations()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var stations = new List<StationDto>
            {
                new StationDto { Id = Guid.NewGuid(), ProsjektId = projectId }
            };
            var res = new StationResult { IsSuccess = true, Stations = stations };
            _stationMock
                .Setup(s => s.GetStationsByProjectIdAsync(projectId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.GetAllStations(projectId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(stations);
        }

        [Fact]
        public async Task GetAllStations_Failure_ReturnsNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var res = new StationResult { IsSuccess = false, Message = "Ingen stasjoner" };
            _stationMock
                .Setup(s => s.GetStationsByProjectIdAsync(projectId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.GetAllStations(projectId);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Ingen stasjoner");
        }

        [Fact]
        public async Task GetStations_Success_ReturnsOkWithStations()
        {
            // Arrange
            var projectId       = Guid.NewGuid();
            var samplingPlanId  = Guid.NewGuid();
            var stations        = new List<StationDto>{ new StationDto() };
            var res = new StationResult { IsSuccess = true, Stations = stations };
            _stationMock
                .Setup(s => s.GetStationsByProvetakningsplanIdAsync(samplingPlanId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.GetStations(projectId, samplingPlanId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(stations);
        }

        [Fact]
        public async Task GetStations_Failure_ReturnsNotFound()
        {
            // Arrange
            var projectId      = Guid.NewGuid();
            var samplingPlanId = Guid.NewGuid();
            var res            = new StationResult { IsSuccess = false, Message = "Feil" };
            _stationMock
                .Setup(s => s.GetStationsByProvetakningsplanIdAsync(samplingPlanId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.GetStations(projectId, samplingPlanId);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Feil");
        }

        [Fact]
        public async Task GetAStation_Success_ReturnsOkWithStation()
        {
            // Arrange
            var projectId      = Guid.NewGuid();
            var samplingPlanId = Guid.NewGuid();
            var stationId      = Guid.NewGuid();
            var station        = new StationDto { Id = stationId };
            var res            = new StationResult { IsSuccess = true, Station = station };
            _stationMock
                .Setup(s => s.GetStationByIdAsync(projectId, stationId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.GetAStation(projectId, samplingPlanId, stationId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(station);
        }

        [Fact]
        public async Task GetAStation_Failure_ReturnsNotFound()
        {
            // Arrange
            var projectId      = Guid.NewGuid();
            var samplingPlanId = Guid.NewGuid();
            var stationId      = Guid.NewGuid();
            var res            = new StationResult { IsSuccess = false, Message = "Ikke funnet" };
            _stationMock
                .Setup(s => s.GetStationByIdAsync(projectId, stationId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.GetAStation(projectId, samplingPlanId, stationId);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Ikke funnet");
        }

        [Fact]
        public async Task GetStationById_ReturnsDtoOrNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var dto       = new BStationDto { Id = stationId };
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projectId, stationId))
                .ReturnsAsync(dto);

            // Act
            var result = await _ctrl.GetStationById(projectId, stationId);

            // Assert
            result.Should().Be(dto);

            // Arrange failure
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projectId, stationId))
                .ReturnsAsync((BStationDto?)null);

            // Act
            var result2 = await _ctrl.GetStationById(projectId, stationId);

            // Assert
            result2.Should().BeNull();
        }

        [Fact]
        public async Task UpdateStation_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Dybde", "Feil");
            var dto = new UpdateStationDto();

            // Act
            var result = await _ctrl.UpdateStation(Guid.NewGuid(), Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateStation_Success_ReturnsOk()
        {
            // Arrange
            var stationId = Guid.NewGuid();
            var dto       = new UpdateStationDto
            {
                Dybde        = 5,
                Analyser     = "X",
                NorthDegree  = 1,
                NorthMinutes = 0.5f,
                EastDegree   = 2,
                EastMinutes  = 0.5f
            };
            var res = new StationResult { IsSuccess = true, Message = "OK" };
            _stationMock
                .Setup(s => s.UpdateStationAsync(stationId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.UpdateStation(Guid.NewGuid(), stationId, dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("OK");
        }

        [Fact]
        public async Task UpdateStation_Failure_ReturnsNotFound()
        {
            // Arrange
            var stationId = Guid.NewGuid();
            var dto       = new UpdateStationDto
            {
                Dybde        = 5,
                Analyser     = "X",
                NorthDegree  = 1,
                NorthMinutes = 0.5f,
                EastDegree   = 2,
                EastMinutes  = 0.5f
            };
            var res = new StationResult { IsSuccess = false, Message = "Ikke funnet" };
            _stationMock
                .Setup(s => s.UpdateStationAsync(stationId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.UpdateStation(Guid.NewGuid(), stationId, dto);

            // Assert
            var nf = result as NotFoundObjectResult;
            nf.Should().NotBeNull();
            nf!.Value.Should().Be("Ikke funnet");
        }

        [Fact]
        public async Task AddStation_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Analyser", "Feil");
            var dto = new NewStationDto();

            // Act
            var result = await _ctrl.AddStation(Guid.NewGuid(), Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task AddStation_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var dto    = new NewStationDto { ProsjektId = Guid.NewGuid() };
            var res    = new StationResult { IsSuccess = false, Message = "Feil" };
            _stationMock
                .Setup(s => s.AddStationToPlanAsync(planId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.AddStation(Guid.NewGuid(), planId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        [Fact]
        public async Task AddStation_Success_ReturnsCreatedAtAction()
        {
            // Arrange
            var projectId      = Guid.NewGuid();
            var samplingPlanId = Guid.NewGuid();
            var dto            = new NewStationDto { ProsjektId = projectId };
            var station        = new StationDto { Id = Guid.NewGuid() };
            var res            = new StationResult { IsSuccess = true, Station = station };
            _stationMock
                .Setup(s => s.AddStationToPlanAsync(samplingPlanId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.AddStation(projectId, samplingPlanId, dto);

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.ActionName.Should().Be(nameof(_ctrl.GetAStation));
            created.RouteValues!["projectId"].Should().Be(projectId);
            created.RouteValues!["samplingPlanId"].Should().Be(samplingPlanId);
            created.RouteValues!["stationId"].Should().Be(station.Id);
            created.Value.Should().Be(station);
        }

        [Fact]
        public async Task AddStationToProject_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Analyser", "Feil");
            var dto = new NewStationDto();

            // Act
            var result = await _ctrl.AddStationToProject(Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task AddStationToProject_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto       = new NewStationDto { ProsjektId = projectId };
            var res       = new StationResult { IsSuccess = false, Message = "Feil" };
            _stationMock
                .Setup(s => s.AddStationToProjectAsync(projectId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.AddStationToProject(projectId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        [Fact]
        public async Task AddStationToProject_Success_ReturnsCreatedAtAction()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var dto       = new NewStationDto { ProsjektId = projectId };
            var station   = new StationDto { Id = Guid.NewGuid() };
            var res       = new StationResult { IsSuccess = true, Station = station };
            _stationMock
                .Setup(s => s.AddStationToProjectAsync(projectId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.AddStationToProject(projectId, dto);

            // Assert
            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.ActionName.Should().Be(nameof(_ctrl.GetStationById));
            created.RouteValues!["projectId"].Should().Be(projectId);
            created.RouteValues!["stationId"].Should().Be(station.Id);
            created.Value.Should().Be(station);
        }

        [Fact]
        public async Task DeleteStation_Success_ReturnsOkWithMessage()
        {
            // Arrange
            var stationId = Guid.NewGuid();
            var res       = new StationResult { IsSuccess = true, Message = "Slettet" };
            _stationMock
                .Setup(s => s.DeleteStationAsync(stationId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.DeleteStation(Guid.NewGuid(), stationId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("Slettet");
        }

        [Fact]
        public async Task DeleteStation_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var stationId = Guid.NewGuid();
            var res       = new StationResult { IsSuccess = false, Message = "Feil" };
            _stationMock
                .Setup(s => s.DeleteStationAsync(stationId))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.DeleteStation(Guid.NewGuid(), stationId);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        #endregion

        #region Survey Operations

        [Fact]
        public async Task GetSurvey_ProjectNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync((ProjectDto?)null);

            // Act
            var result = await _ctrl.GetSurvey(projId, stationId, surveyId);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Project does not exist");
        }

        [Fact]
        public async Task GetSurvey_StationNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync((BStationDto?)null);

            // Act
            var result = await _ctrl.GetSurvey(projId, stationId, surveyId);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Station does not exist");
        }

        [Fact]
        public async Task GetSurvey_SurveyNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.GetSurveyById(surveyId))
                .ReturnsAsync((EditSurveyDto?)null);

            // Act
            var result = await _ctrl.GetSurvey(projId, stationId, surveyId);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be($"No survey with id {surveyId}");
        }

        [Fact]
        public async Task GetSurvey_Success_ReturnsOkWithDto()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            var dto       = new EditSurveyDto { Id = surveyId };
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.GetSurveyById(surveyId))
                .ReturnsAsync(dto);

            // Act
            var result = await _ctrl.GetSurvey(projId, stationId, surveyId);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(dto);
        }

        [Fact]
        public async Task CreateSurvey_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Feltdato", "Feil");
            var dto = new EditSurveyDto();

            // Act
            var result = await _ctrl.CreateSurvey(Guid.NewGuid(), Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task CreateSurvey_ProjectNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync((ProjectDto?)null);

            // Act
            var result = await _ctrl.CreateSurvey(projId, stationId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Project does not exist");
        }

        [Fact]
        public async Task CreateSurvey_StationNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync((BStationDto?)null);

            // Act
            var result = await _ctrl.CreateSurvey(projId, stationId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Station does not exist");
        }

        [Fact]
        public async Task CreateSurvey_Success_ReturnsOkWithMessage()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            var res       = new EditSurveyResult { IsSuccess = true, Message = "OK" };
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.CreateSurvey(projId, stationId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.CreateSurvey(projId, stationId, dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("OK");
        }

        [Fact]
        public async Task CreateSurvey_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            var res       = new EditSurveyResult { IsSuccess = false, Message = "Feil" };
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.CreateSurvey(projId, stationId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.CreateSurvey(projId, stationId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        [Fact]
        public async Task UpdateSurvey_ModelStateInvalid_ReturnsBadRequest()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Feltdato", "Feil");
            var dto = new EditSurveyDto();

            // Act
            var result = await _ctrl.UpdateSurvey(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateSurvey_ProjectNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync((ProjectDto?)null);

            // Act
            var result = await _ctrl.UpdateSurvey(projId, stationId, surveyId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Project does not exist");
        }

        [Fact]
        public async Task UpdateSurvey_StationNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync((BStationDto?)null);

            // Act
            var result = await _ctrl.UpdateSurvey(projId, stationId, surveyId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Station does not exist");
        }

        [Fact]
        public async Task UpdateSurvey_SurveyNull_ReturnsBadRequest()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.GetSurveyById(surveyId))
                .ReturnsAsync((EditSurveyDto?)null);

            // Act
            var result = await _ctrl.UpdateSurvey(projId, stationId, surveyId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be($"No survey with id {surveyId}");
        }

        [Fact]
        public async Task UpdateSurvey_Success_ReturnsOkWithMessage()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            var res       = new EditSurveyResult { IsSuccess = true, Message = "Oppdatert" };
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.GetSurveyById(surveyId))
                .ReturnsAsync(dto);
            _surveyMock
                .Setup(s => s.UpdateSurvey(projId, stationId, surveyId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.UpdateSurvey(projId, stationId, surveyId, dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be("Oppdatert");
        }

        [Fact]
        public async Task UpdateSurvey_Failure_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var projId    = Guid.NewGuid();
            var stationId = Guid.NewGuid();
            var surveyId  = Guid.NewGuid();
            var dto       = new EditSurveyDto();
            var res       = new EditSurveyResult { IsSuccess = false, Message = "Feil" };
            _projectMock
                .Setup(s => s.GetProjectByIdAsync(projId))
                .ReturnsAsync(new ProjectDto());
            _stationMock
                .Setup(s => s.GetBStationDtoByStationId(projId, stationId))
                .ReturnsAsync(new BStationDto());
            _surveyMock
                .Setup(s => s.GetSurveyById(surveyId))
                .ReturnsAsync(dto);
            _surveyMock
                .Setup(s => s.UpdateSurvey(projId, stationId, surveyId, dto))
                .ReturnsAsync(res);

            // Act
            var result = await _ctrl.UpdateSurvey(projId, stationId, surveyId, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil");
        }

        #endregion
    }