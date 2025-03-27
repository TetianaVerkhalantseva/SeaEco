using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.User;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Server.Infrastructure;
using SeaEco.Services.UserServices;

namespace SeaEco.Server.Controllers;

[Route("/api/users")]
[Authorize]
[RoleAccessor(true)]

public class UserController(IUserService userService, ICurrentUserContext currentUserContext) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] bool? isActive)
    {
        IEnumerable<UserDto> users = await userService.GetAllUsers(isActive);
        return users.Any() ? AsOk(users) : AsOk();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        Response<UserDto> response = await userService.GetUserById(id);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }

    [HttpPut("{id:guid}/update")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] EditUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        Response response = await userService.Update(id, dto);
        return response.IsError
            ? BadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpPut("{id:guid}/update/active")]
    public async Task<IActionResult> ToggleActive([FromRoute] Guid id)
    {
        Response response = await userService.ToggleActive(id);
        return response.IsError
            ? BadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        Response<UserDto> response = await userService.GetUserById(currentUserContext.Id);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk(response.Value);
    }
}