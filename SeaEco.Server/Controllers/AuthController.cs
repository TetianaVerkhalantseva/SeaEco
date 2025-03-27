using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.Server.Infrastructure;
using SeaEco.Services.AuthServices;
using SeaEco.Services.TokenServices;
using System.Security.Claims;
using SeaEco.Services.TokenServices;

namespace SeaEco.Server.Controllers;

[Route("/api/authentication")]
public class AuthController(IAuthService authService, ITokenService tokenService) : ApiControllerBase
{
    [HttpPost("register")]
    [RoleAccessor(true)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Response<string> response = await authService.RegisterUser(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
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

    [HttpPost("request-reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Response response = await authService.ResetPasswordRequest(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordConfirmDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Response response = await authService.ResetPasswordConfirm(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Response response = await authService.ChangePassword(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpGet("validate-token/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateToken([FromRoute] string token)
    {
        Response response = await tokenService.Validate(token);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }
    
    // The endpoint to test and demonstrate saving the JWT token in cookies.
    [HttpGet("test")]
    [RoleAccessor(true)]
    public async Task<IActionResult> Test()
    {
        Response<Bruker> userResult = await authService.GetCurrentUser();
        return userResult.IsError
            ? AsBadRequest(userResult.ErrorMessage)
            : AsOk(userResult.Value);
    }
   
    [HttpGet("isAuthenticated")]
    public async Task<IActionResult> GetAuthenticated()
    {
        bool isAuthenticated = User?.Identity != null && User.Identity.IsAuthenticated;
        List<string> roles = new List<string>();

        if (isAuthenticated)
        {
            roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }
        
        return Ok(new { isAuthenticated,roles });
    }
}