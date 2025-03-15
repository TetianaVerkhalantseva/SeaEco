using SeaEco.Abstractions.Models.Authentication;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.AuthServices;

public interface IAuthService
{
    Task<Response<string>> RegisterUser(RegisterUserDto dto);
    Task<Response<string>> SignIn(LoginDto dto);

    Task<Response> ResetPasswordRequest(ResetPasswordDto dto);
    Task<Response> ResetPasswordConfirm(ResetPasswordConfirmDto dto);
    Task<Response> ChangePassword(ChangePasswordDto dto);
    
    void SignOut();
    
    // Only for test
    Task<Response<Bruker>> GetCurrentUser();
}