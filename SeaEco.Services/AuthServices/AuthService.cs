using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SeaEco.Abstractions.Models.Authentication;
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
    
    public async Task<Response<string>> RegisterUser(RegisterUserDto dto)
    {
        Response<Bruker> userResult = await GetUser(dto.Email);
        if (!userResult.IsError)
        {
            return Response<string>.Error(WasCreatedError);
        }
        
        var passwordResult = Hasher.Hash(dto.Password);

        Bruker user = new()
        {
            PassordHash = passwordResult.hashed,
            Salt = Encoding.UTF8.GetString(passwordResult.salt),
            Aktiv = true,
            Id = Guid.NewGuid(),
            Datoregistrert = DateTime.Now,
            Fornavn = dto.FirstName,
            Etternavn = dto.LastName,
            Epost = dto.Email,
            IsAdmin = dto.IsAdmin
        };
        
        Response createResult = await userRepository.Add(user);
        if (createResult.IsError)
        {
            return Response<string>.Error(RegistrationError);
        }

        return await AuthenticationProcess(user);
    }

    public async Task<Response<string>> SignIn(string login, string password)
    {
        Response<Bruker> userResult = await GetUser(login, password);
        if (userResult.IsError)
        {
            return Response<string>.Error(userResult.ErrorMessage);
        }

        return await AuthenticationProcess(userResult.Value);
    }

    public Task SignOut() => httpContextAccessor.HttpContext!.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

    private async Task<Response<string>> AuthenticationProcess(Bruker bruker)
    {
        IEnumerable<Claim> claims = GetClaims(bruker);
        ClaimsPrincipal principle = GetPrincipal(claims);

        try
        {
            await httpContextAccessor.HttpContext!.SignInAsync(JwtBearerDefaults.AuthenticationScheme, principle);
        }
        catch
        {
            return Response<string>.Error(AuthenticationFailedError);
        }
        
        return jwtService.Encode(claims);
    }
    
    private async Task<Response<Bruker>> GetUser(string email)
    {
        Bruker? dbRecord = await userRepository.GetBy(user => user.Epost == email);
        return dbRecord is null
            ? Response<Bruker>.Error(InvalidCredentialsError)
            : Response<Bruker>.Ok(dbRecord);
    }
    
    private async Task<Response<Bruker>> GetUser(string email, string password)
    {
        Response<Bruker> userResult = await GetUser(email);
        if (userResult.IsError)
        {
            return userResult;
        }
        
        Bruker dbRecord = userResult.Value;

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