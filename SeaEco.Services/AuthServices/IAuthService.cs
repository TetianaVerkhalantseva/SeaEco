using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.AuthServices;

public interface IAuthService
{
    Task<Response<string>> RegisterUser(RegisterUserDto dto);
    Task<Response<string>> SignIn(LoginDto dto);
    void SignOut();
}