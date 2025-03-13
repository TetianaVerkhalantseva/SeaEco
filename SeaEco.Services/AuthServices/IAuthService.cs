using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.AuthServices;

public interface IAuthService
{
    Task<Response<string>> RegisterUser(RegisterUserDto dto);
    
    Task<Response<string>> SignIn(string login, string password);
    
    Task SignOut();
}