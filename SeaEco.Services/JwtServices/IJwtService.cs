using System.Security.Claims;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.JwtServices;

public interface IJwtService
{
    public int Expiration { get; }
    Response<string> Encode(IEnumerable<Claim> claims);
    Response<string> RandomToken();
    Response<IEnumerable<Claim>> Decode(string token);
}