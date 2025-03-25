using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SeaEco.Server.Controllers;

[ApiController]
[Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult AsOk(object value) => Ok(value);
    protected IActionResult AsOk() => Ok();
    protected IActionResult AsBadRequest(string errorMessage) => BadRequest(new { errorMessage = errorMessage });
}