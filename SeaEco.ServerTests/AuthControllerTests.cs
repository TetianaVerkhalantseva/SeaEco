namespace SeaEco.ServerTests;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.Server.Controllers;
using SeaEco.Services.AuthServices;
using SeaEco.Services.TokenServices;
using Xunit;


public class AuthControllerTests
    {
        private readonly Mock<IAuthService>  _authMock;
        private readonly Mock<ITokenService> _tokenMock;
        private readonly AuthController      _ctrl;

        public AuthControllerTests()
        {
            _authMock  = new Mock<IAuthService>();
            _tokenMock = new Mock<ITokenService>();
            _ctrl      = new AuthController(_authMock.Object, _tokenMock.Object);
            
            _ctrl.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }
        

        [Fact]
        public async Task Register_ValidDto_ReturnsOk()
        {
            var dto = new RegisterUserDto
            {
                FirstName       = "Ola",
                LastName        = "Nordmann",
                Email           = "ola@n.no",
                Password        = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                IsAdmin         = false
            };
            _authMock.Setup(s => s.RegisterUser(dto))
                     .ReturnsAsync(Response<string>.Ok("Bruker registrert"));

            var result = await _ctrl.Register(dto);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Register_ModelStateInvalid_ReturnsBadRequestModelState()
        {
            _ctrl.ModelState.AddModelError("Email", "Må være en gyldig epost");

            var dto = new RegisterUserDto(); 
            var result = await _ctrl.Register(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task Register_ServiceError_ReturnsBadRequestWithMessage()
        {
            var dto = new RegisterUserDto
            {
                FirstName       = "Ola",
                LastName        = "Nordmann",
                Email           = "ola@n.no",
                Password        = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                IsAdmin         = false
            };
            _authMock.Setup(s => s.RegisterUser(dto))
                     .ReturnsAsync(Response<string>.Error("Bruker eksisterer"));

            var result = await _ctrl.Register(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Bruker eksisterer" });
        }
        
        [Fact]
        public async Task Login_ValidDto_ReturnsOk()
        {
            var dto = new LoginDto
            {
                Email    = "ola@n.no",
                Password = "Passw0rd!"
            };
            _authMock.Setup(s => s.SignIn(dto))
                     .ReturnsAsync(Response<string>.Ok("token"));

            var result = await _ctrl.Login(dto);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Login_ModelStateInvalid_ReturnsBadRequestModelState()
        {
            _ctrl.ModelState.AddModelError("Password", "Passord mangler");

            var dto = new LoginDto();
            var result = await _ctrl.Login(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task Login_ServiceError_ReturnsBadRequestWithMessage()
        {
            var dto = new LoginDto
            {
                Email    = "ola@n.no",
                Password = "Feil!"
            };
            _authMock.Setup(s => s.SignIn(dto))
                     .ReturnsAsync(Response<string>.Error("Ugyldige credentials"));

            var result = await _ctrl.Login(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Ugyldige credentials" });
        }
        

        [Fact]
        public async Task Logout_Always_CallsSignOutAndReturnsOk()
        {
            var result = await _ctrl.Logout();

            _authMock.Verify(s => s.SignOut(), Times.Once);
            result.Should().BeOfType<OkResult>();
        }
        

        [Fact]
        public async Task RequestResetPassword_ValidDto_ReturnsOk()
        {
            var dto = new ResetPasswordDto { Email = "ola@n.no" };
            _authMock.Setup(s => s.ResetPasswordRequest(dto))
                     .ReturnsAsync(Response.Ok());

            var result = await _ctrl.RequestResetPassword(dto);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RequestResetPassword_ModelStateInvalid_ReturnsBadRequestModelState()
        {
            _ctrl.ModelState.AddModelError("Email", "Må være epost");

            var dto = new ResetPasswordDto();
            var result = await _ctrl.RequestResetPassword(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task RequestResetPassword_ServiceError_ReturnsBadRequestWithMessage()
        {
            var dto = new ResetPasswordDto { Email = "ola@n.no" };
            _authMock.Setup(s => s.ResetPasswordRequest(dto))
                     .ReturnsAsync(Response.Error("Feil ved reset"));

            var result = await _ctrl.RequestResetPassword(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Feil ved reset" });
        }
        

        [Fact]
        public async Task ResetPassword_ValidDto_ReturnsOk()
        {
            var dto = new ResetPasswordConfirmDto
            {
                Password        = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Token           = "tkn"
            };
            _authMock.Setup(s => s.ResetPasswordConfirm(dto))
                     .ReturnsAsync(Response.Ok());

            var result = await _ctrl.ResetPassword(dto);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ResetPassword_ModelStateInvalid_ReturnsBadRequestModelState()
        {
            _ctrl.ModelState.AddModelError("Password", "Mangler");

            var dto = new ResetPasswordConfirmDto();
            var result = await _ctrl.ResetPassword(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task ResetPassword_ServiceError_ReturnsBadRequestWithMessage()
        {
            var dto = new ResetPasswordConfirmDto
            {
                Password        = "Passw0rd!",
                ConfirmPassword = "Passw0rd!",
                Token           = "tkn"
            };
            _authMock.Setup(s => s.ResetPasswordConfirm(dto))
                     .ReturnsAsync(Response.Error("Ugyldig token"));

            var result = await _ctrl.ResetPassword(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Ugyldig token" });
        }

        [Fact]
        public async Task ChangePassword_ValidDto_ReturnsOk()
        {
            var dto = new ChangePasswordDto
            {
                OldPassword     = "Gammelt!",
                NewPassword     = "NyttPass1!",
                ConfirmPassword = "NyttPass1!"
            };
            _authMock.Setup(s => s.ChangePassword(dto))
                     .ReturnsAsync(Response.Ok());

            var result = await _ctrl.ChangePassword(dto);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ChangePassword_ModelStateInvalid_ReturnsBadRequestModelState()
        {
            _ctrl.ModelState.AddModelError("NewPassword", "Mangler");

            var dto = new ChangePasswordDto();
            var result = await _ctrl.ChangePassword(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value.Should().BeOfType<SerializableError>();
        }

        [Fact]
        public async Task ChangePassword_ServiceError_ReturnsBadRequestWithMessage()
        {
            var dto = new ChangePasswordDto
            {
                OldPassword     = "Gammelt!",
                NewPassword     = "NyttPass1!",
                ConfirmPassword = "NyttPass1!"
            };
            _authMock.Setup(s => s.ChangePassword(dto))
                     .ReturnsAsync(Response.Error("Kan ikke bytte passord"));

            var result = await _ctrl.ChangePassword(dto);

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Kan ikke bytte passord" });
        }

        [Fact]
        public async Task ValidateToken_Valid_ReturnsOk()
        {
            _tokenMock.Setup(s => s.Validate("tkn"))
                      .ReturnsAsync(Response.Ok());

            var result = await _ctrl.ValidateToken("tkn");

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ValidateToken_Failure_ReturnsBadRequestWithMessage()
        {
            _tokenMock.Setup(s => s.Validate("tkn"))
                      .ReturnsAsync(Response.Error("Ugyldig"));

            var result = await _ctrl.ValidateToken("tkn");

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Ugyldig" });
        }


        [Fact]
        public async Task Test_WhenUserExists_ReturnsOkWithUser()
        {
            var bruker = new Bruker { Id = Guid.NewGuid(), Fornavn = "Ola", Etternavn = "N" };
            _authMock.Setup(s => s.GetCurrentUser())
                     .ReturnsAsync(Response<Bruker>.Ok(bruker));

            var result = await _ctrl.Test();

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value.Should().Be(bruker);
        }

        [Fact]
        public async Task Test_ServiceError_ReturnsBadRequestWithMessage()
        {
            _authMock.Setup(s => s.GetCurrentUser())
                     .ReturnsAsync(Response<Bruker>.Error("Ikke logget inn"));

            var result = await _ctrl.Test();

            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.Value
               .Should().BeEquivalentTo(new { errorMessage = "Ikke logget inn" });
        }

        [Fact]
        public async Task GetAuthenticated_Authenticated_ReturnsTrueAndRoles()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            _ctrl.ControllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            var result = await _ctrl.GetAuthenticated();

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value
               .Should().BeEquivalentTo(new
               {
                   isAuthenticated = true,
                   roles           = new List<string> { "Admin", "User" }
               });
        }

        [Fact]
        public async Task GetAuthenticated_NotAuthenticated_ReturnsFalseAndEmptyRoles()
        {
            // ingen identity → IsAuthenticated == false
            _ctrl.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            var result = await _ctrl.GetAuthenticated();

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.Value
               .Should().BeEquivalentTo(new
               {
                   isAuthenticated = false,
                   roles           = new List<string>()
               });
        }
    }
