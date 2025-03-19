using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.User;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;

namespace SeaEco.Services.UserServices;

public sealed class UserService(IGenericRepository<Bruker> userRepository) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllUsers(bool? isActive)
    {
        IQueryable<Bruker> query = userRepository.GetAll();
        IEnumerable<Bruker> users = [];
        
        if (isActive is null)
        {
            users = await query.ToListAsync();
        }

        if (isActive == true)
        {
            users = await query.Where(record => record.Aktiv).ToListAsync();
        }

        if (isActive == false)
        {
            users = await query.Where(record => !record.Aktiv).ToListAsync();
        }

        return users.Select(user => new UserDto()
        {
            Id = user.Id,
            FirstName = user.Fornavn,
            LastName = user.Etternavn,
            Email = user.Epost,
            IsAdmin = user.IsAdmin,
            IsActive = user.Aktiv,
            Datoregistrert = user.Datoregistrert
        });
    }

    public Task<Response<UserDto>> GetUserById(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Response> Update(EditUserDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<Response> ToggleActive(Guid userId)
    {
        throw new NotImplementedException();
    }
}