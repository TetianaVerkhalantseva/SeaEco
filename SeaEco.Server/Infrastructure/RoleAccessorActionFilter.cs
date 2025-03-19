using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeaEco.Services.JwtServices;

namespace SeaEco.Server.Infrastructure;

public class RoleAccessorActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var roleAttr = context.ActionDescriptor.EndpointMetadata
            .OfType<RoleAccessorAttribute>()
            .FirstOrDefault();

        if (roleAttr is null)
        {
            await next();
            return;
        }
        
        string? value = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.ADMIN)?.Value;
        if (string.IsNullOrEmpty(value))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!bool.TryParse(value, out bool isAdmin))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (roleAttr.IsAdmin && isAdmin)
        {
            await next();
        }
        else
        {
            context.Result = new ForbidResult();
        }
    }
}