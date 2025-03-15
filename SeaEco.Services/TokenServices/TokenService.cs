using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.JwtServices;

namespace SeaEco.Services.TokenServices;

public class TokenService(IJwtService jwtService, IGenericRepository<Token> tokenRepository) : ITokenService
{
    private const string CreateTokenError = "Error while generated token";
    private const string TokenNotFoundError = "Token not found";
    
    public async Task<Response<string>> CreateToken(IEnumerable<Claim> claims, Guid userId)
    {
        Response<string> tokenResult = jwtService.Encode(claims);
        if (tokenResult.IsError)
        {
            return tokenResult;
        }

        Token dbRecord = new Token()
        {
            Id = Guid.NewGuid(),
            Token1 = tokenResult.Value,
            BrukerId = userId,
            CreatedAt = DateTime.Now,
            ExpiredAt = DateTime.Now.AddSeconds(jwtService.Expiration),
            IsUsed = false,
        };
        
        Response createResult = await tokenRepository.Add(dbRecord);
        if (createResult.IsError)
        {
            return Response<string>.Error(CreateTokenError);
        }

        return Response<string>.Ok(tokenResult.Value);
    }

    public async Task<Response<Token>> GetByPayload(string payload)
    {
        Token? dbRecord = await tokenRepository.GetAll()
            .Include(token => token.Bruker)
            .FirstOrDefaultAsync(token => token.Token1 == payload);

        return dbRecord is null
            ? Response<Token>.Error(TokenNotFoundError)
            : Response<Token>.Ok(dbRecord);
    }
}