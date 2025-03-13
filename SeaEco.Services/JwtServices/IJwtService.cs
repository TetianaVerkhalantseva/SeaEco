using System.Security.Claims;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.JwtServices;

public interface IJwtService
{
    Response<string> Encode(IEnumerable<Claim> claims);
    Response<string> RandomToken();
    Response<IEnumerable<Claim>> Decode(string token);
}