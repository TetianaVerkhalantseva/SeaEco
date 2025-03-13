using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.AuthServices;

public interface IAuthService
{
    Task<Response<string>> SignIn(string login, string password);
    
    Task SignOut();
}