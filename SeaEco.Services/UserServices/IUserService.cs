using SeaEco.Abstractions.Models.User;
using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.UserServices;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsers(bool? isActive);
    Task<Response<UserDto>> GetUserById(Guid userId);
    Task<Response> Update(Guid id, EditUserDto dto);
    Task<Response> ToggleActive(Guid userId);
}
