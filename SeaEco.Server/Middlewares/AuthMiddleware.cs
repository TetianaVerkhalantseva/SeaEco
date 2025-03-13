using Microsoft.AspNetCore.Authorization;

namespace SeaEco.Server.Middlewares;

public sealed class AuthMiddleware(RequestDelegate next)
{
    private const string TokenCookieName = "auth_token";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var hasAuthorize = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

        if (!hasAuthorize)
        {
            await next(context);
            return;
        }
        
        string? token = context.Request.Cookies[TokenCookieName];
        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Request.Headers.Add("Authorization", $"Bearer {token}");
        }
        
        await next(context);
    }
}