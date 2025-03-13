using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.HashService;
using SeaEco.Services.JwtServices;

namespace SeaEco.Services.AuthServices;
public sealed class AuthService(
    IHttpContextAccessor httpContextAccessor,
    IGenericRepository<Bruker> userRepository,
    IJwtService jwtService)
    : IAuthService
{
    private const string InvalidCredentialsError = "Invalid credentials.";
    private const string WasCreatedError = "Was created.";
    private const string RegistrationError = "Error while registration user.";
    private const string AuthenticationFailedError = "Authentication failed.";

    public async Task<Response<string>> SignIn(string login, string password)
    {
        Response<Bruker> userResult = await GetUser(login, password);
        if (userResult.IsError)
        {
            return Response<string>.Error(userResult.ErrorMessage);
        }
        
        IEnumerable<Claim> claims = GetClaims(userResult.Value);
        ClaimsPrincipal principle = GetPrincipal(claims);
        
        await httpContextAccessor.HttpContext!.SignInAsync(JwtBearerDefaults.AuthenticationScheme, principle);
        
        return jwtService.Encode(claims);
    }

    public Task SignOut() => httpContextAccessor.HttpContext!.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

    private async Task<Response<Bruker>> GetUser(string email, string password)
    {
        Bruker? dbRecord = await userRepository.GetBy(user => user.Epost == email);
        if (dbRecord is null)
        {
            return Response<Bruker>.Error(InvalidCredentialsError);
        }

        if (Hasher.Verify(dbRecord.PassordHash, password, Encoding.UTF8.GetBytes(dbRecord.Salt)))
        {
            return Response<Bruker>.Ok(dbRecord);
        }
        
        return Response<Bruker>.Error(InvalidCredentialsError);
    }

    private IEnumerable<Claim> GetClaims(Bruker user) =>
    [
        new(JwtClaimTypes.SUB, user.Id.ToString()),
        new(JwtClaimTypes.NAME, user.Epost),
        new(JwtClaimTypes.IAT, DateTimeOffset.Now.Ticks.ToString())
    ];

    private ClaimsPrincipal GetPrincipal(IEnumerable<Claim> claims) => new(new ClaimsIdentity(claims));

}