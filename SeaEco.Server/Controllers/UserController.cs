using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.User;
using SeaEco.Services.UserServices;

namespace SeaEco.Server.Controllers;

[Route("/api/users")]
public class UserController(IUserService userService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] bool? isActive)
    {
        IEnumerable<UserDto> users = await userService.GetAllUsers(isActive);
        return users.Any() ? AsOk(users) : AsOk();
    }
}