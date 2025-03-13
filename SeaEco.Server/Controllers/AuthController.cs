using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.AuthServices;

namespace SeaEco.Server.Controllers;

[Route("/api/authentication")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        Response<string> response = await authService.RegisterUser(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        Response<string> response = await authService.SignIn(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        authService.SignOut();
        return AsOk();
    }
    
    // The endpoint to test and demonstrate saving the JWT token in cookies.
    [HttpGet("test")]
    [Authorize]
    public async Task<IActionResult> Test() => AsOk();
}