using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.EmailServices;
using SeaEco.Services.EmailServices.Models;
using SeaEco.Services.HashService;
using SeaEco.Services.JwtServices;
using SeaEco.Services.TokenServices;
using SeaEco.Services.UserServices;

namespace SeaEco.Services.AuthServices;
public sealed class AuthService(
    IHttpContextAccessor httpContextAccessor,
    IGenericRepository<Bruker> userRepository,
    ITokenService tokenService,
    IEmailService emailService,
    IJwtService jwtService,
    ICurrentUserContext currentUserContext)
    : IAuthService
{
    private const string InvalidCredentialsError = "Invalid credentials.";
    private const string WasCreatedError = "Was created.";
    private const string RegistrationError = "Error while registration user.";
    private const string TokenCookieName = "auth_token";
    
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
            Salt = passwordResult.salt,
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

        return AuthenticationProcess(user, false);
    }

    public async Task<Response<string>> SignIn(LoginDto dto)
    {
        Response<Bruker> userResult = await GetUser(dto.Email, dto.Password);
        if (userResult.IsError)
        {
            return Response<string>.Error(userResult.ErrorMessage);
        }

        return AuthenticationProcess(userResult.Value);
    }

    public async Task<Response> ResetPasswordRequest(ResetPasswordDto dto)
    {
        Response<Bruker> getUserResult = await GetUser(dto.Email);
        if (getUserResult.IsError)
        {
            return Response.Error(getUserResult.ErrorMessage);
        }
        
        IEnumerable<Claim> claims = GetClaims(getUserResult.Value);
        Response<string> tokenResult = await tokenService.CreateToken(claims, getUserResult.Value.Id);
        if (tokenResult.IsError)
        {
            return Response.Error(tokenResult.ErrorMessage);
        }

        string url = UrlBuilder.ResetPasswordUrl(tokenResult.Value);

        return await emailService.SendMail(new EmailMessageModel()
        {
            Content = $"Follow the <a href='{url}'>link</a>",
            Recipients = [getUserResult.Value.Epost],
            Subject = "Reset password",
            BodyType = EmailBodyType.Html,
        });
    }

    public async Task<Response> ResetPasswordConfirm(ResetPasswordConfirmDto dto)
    {
        Response<Token> tokenResult = await tokenService.GetByPayload(dto.Token);
        if (tokenResult.IsError)
        {
            return Response.Error(tokenResult.ErrorMessage);
        }
        
        var password = Hasher.Hash(dto.Password);
        
        Bruker user = tokenResult.Value.Bruker;
        user.PassordHash = password.hashed;
        user.Salt = password.salt;
        
        return await userRepository.Update(user);
    }

    public async Task<Response> ChangePassword(ChangePasswordDto dto)
    {
        Response<Bruker> userResult = await GetUser(currentUserContext.Email);
        if (userResult.IsError)
        {
            return Response.Error(userResult.ErrorMessage);
        }
        
        Bruker user = userResult.Value;
        if (!Hasher.Verify(user.PassordHash, dto.OldPassword, user.Salt))
        {
            return Response.Error(InvalidCredentialsError);
        }

        var password = Hasher.Hash(dto.NewPassword);
        
        user.PassordHash = password.hashed;
        user.Salt = password.salt;

        return await userRepository.Update(user);
    }

    public void SignOut()
    {
        string? token = httpContextAccessor.HttpContext?.Request.Cookies[TokenCookieName];
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(TokenCookieName);
    }

    public Task<Response<Bruker>> GetCurrentUser() => GetUser(currentUserContext.Email);

    private Response<string> AuthenticationProcess(Bruker bruker, bool appendToken = true)
    {
        IEnumerable<Claim> claims = GetClaims(bruker);
        
        Response<string> tokenResult = jwtService.Encode(claims);
        if (tokenResult.IsError)
        {
            return tokenResult;
        }

        if (appendToken)
        {
            httpContextAccessor.HttpContext?.Response.Cookies.Append(TokenCookieName, tokenResult.Value, new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddSeconds(jwtService.Expiration)
            });
        }
        
        return tokenResult;
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

        if (Hasher.Verify(dbRecord.PassordHash, password, dbRecord.Salt))
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