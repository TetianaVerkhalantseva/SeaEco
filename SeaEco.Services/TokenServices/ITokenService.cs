using System.Security.Claims;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.TokenServices;

public interface ITokenService
{
    Task<Response<string>> CreateToken(IEnumerable<Claim> claims, Guid userId);
    Task<Response<Token>> GetByPayload(string payload);
}