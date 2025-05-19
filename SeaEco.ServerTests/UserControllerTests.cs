using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaEco.Abstractions.Models.User;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Server.Controllers;
using SeaEco.Services.UserServices;

namespace SeaEco.ServerTests;

public class UserControllerTests
{
    private readonly Mock<IUserService>          _userServiceMock;
        private readonly Mock<ICurrentUserContext>   _currentUserContextMock;
        private readonly UserController             _ctrl;

        public UserControllerTests()
        {
            _userServiceMock        = new Mock<IUserService>();
            _currentUserContextMock = new Mock<ICurrentUserContext>();
            _ctrl                   = new UserController(_userServiceMock.Object, _currentUserContextMock.Object);

            
            _ctrl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        #region GetUsers

        [Fact]
        public async Task GetUsers_WhenUsersExist_ReturnsOkObjectResultWithList()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Ola",
                    LastName = "Nordmann",
                    Email = "ola@n.no",
                    IsAdmin = false,
                    IsActive = true
                }
            };
            _userServiceMock
                .Setup(s => s.GetAllUsers(null))
                .ReturnsAsync(users);

            // Act
            var result = await _ctrl.GetUsers(null);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetUsers_WhenNoUsers_ReturnsOkResult()
        {
            // Arrange
            _userServiceMock
                .Setup(s => s.GetAllUsers(true))
                .ReturnsAsync(new List<UserDto>());

            // Act
            var result = await _ctrl.GetUsers(true);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        #endregion

        #region GetUser

        [Fact]
        public async Task GetUser_WhenFound_ReturnsOkObjectResultWithDto()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new UserDto
            {
                Id = id,
                FirstName = "Kari",
                LastName = "Nordmann",
                Email = "kari@n.no",
                IsAdmin = true,
                IsActive = true
            };
            _userServiceMock
                .Setup(s => s.GetUserById(id))
                .ReturnsAsync(Response<UserDto>.Ok(dto));

            // Act
            var result = await _ctrl.GetUser(id);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(dto);
        }

        [Fact]
        public async Task GetUser_WhenServiceError_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _userServiceMock
                .Setup(s => s.GetUserById(id))
                .ReturnsAsync(Response<UserDto>.Error("Bruker ikke funnet"));

            // Act
            var result = await _ctrl.GetUser(id);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Bruker ikke funnet" });
        }

        #endregion

        #region UpdateUser

        [Fact]
        public async Task UpdateUser_WhenModelStateInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _ctrl.ModelState.AddModelError("Email", "Påkrevd");
            var dto = new EditUserDto();

            // Act
            var result = await _ctrl.UpdateUser(Guid.NewGuid(), dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task UpdateUser_WhenSuccess_ReturnsOkResult()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditUserDto
            {
                FirstName = "Ola",
                LastName  = "Nordmann",
                Email     = "ola@n.no",
                IsAdmin   = false
            };
            _userServiceMock
                .Setup(s => s.Update(id, dto))
                .ReturnsAsync(Response.Ok());

            // Act
            var result = await _ctrl.UpdateUser(id, dto);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateUser_WhenServiceError_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new EditUserDto
            {
                FirstName = "Ola",
                LastName  = "Nordmann",
                Email     = "ola@n.no",
                IsAdmin   = false
            };
            _userServiceMock
                .Setup(s => s.Update(id, dto))
                .ReturnsAsync(Response.Error("Feil ved oppdatering"));

            // Act
            var result = await _ctrl.UpdateUser(id, dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Feil ved oppdatering");
        }

        #endregion

        #region ToggleActive

        [Fact]
        public async Task ToggleActive_WhenSuccess_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            _userServiceMock
                .Setup(s => s.ToggleActive(id))
                .ReturnsAsync(Response.Ok());

            // Act
            var result = await _ctrl.ToggleActive(id);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ToggleActive_WhenServiceError_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _userServiceMock
                .Setup(s => s.ToggleActive(id))
                .ReturnsAsync(Response.Error("Kunne ikke endre aktiv-status"));

            // Act
            var result = await _ctrl.ToggleActive(id);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().Be("Kunne ikke endre aktiv-status");
        }

        #endregion

        #region GetCurrentUser

        [Fact]
        public async Task GetCurrentUser_WhenFound_ReturnsOkObjectResultWithDto()
        {
            // Arrange
            var id  = Guid.NewGuid();
            var dto = new UserDto
            {
                Id = id,
                FirstName = "Test",
                LastName  = "Bruker",
                Email     = "test@bruker.no",
                IsAdmin   = false,
                IsActive  = true
            };
            _currentUserContextMock
                .Setup(c => c.Id)
                .Returns(id);
            _userServiceMock
                .Setup(s => s.GetUserById(id))
                .ReturnsAsync(Response<UserDto>.Ok(dto));

            // Act
            var result = await _ctrl.GetCurrentUser();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(dto);
        }

        [Fact]
        public async Task GetCurrentUser_WhenServiceError_ReturnsBadRequestWithMessage()
        {
            // Arrange
            var id = Guid.NewGuid();
            _currentUserContextMock
                .Setup(c => c.Id)
                .Returns(id);
            _userServiceMock
                .Setup(s => s.GetUserById(id))
                .ReturnsAsync(Response<UserDto>.Error("Ingen gjeldende bruker"));

            // Act
            var result = await _ctrl.GetCurrentUser();

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Ingen gjeldende bruker" });
        }

        #endregion
}