using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SeaEco.Services.JwtServices;

namespace SeaEco.Services.UserServices;

public class CurrentUserContext(IHttpContextAccessor contextAccessor) : ICurrentUserContext
{
    private const string UnauthorizedError = "Unauthorized.";

    public Guid Id
    {
        get
        {
            string? value = contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(value))
            {
                throw new UnauthorizedAccessException(UnauthorizedError);
            }

            if (!Guid.TryParse(value, out Guid id))
            {
                throw new UnauthorizedAccessException(UnauthorizedError);
            }

            return id;
        }
    }

    public string Email
    {
        get
        {
            string? value = contextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.NAME)?.Value;
            if (string.IsNullOrEmpty(value))
            {
                throw new UnauthorizedAccessException(UnauthorizedError);
            }

            return value;
        }
    }
}