using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.User;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;

namespace SeaEco.Services.UserServices;

public sealed class UserService(IGenericRepository<Bruker> userRepository) : IUserService
{
    private const string UserNotFoundError  = "User not found";
    
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

        return users.Select(MapUser);
    }

    public async Task<Response<UserDto>> GetUserById(Guid userId)
    {
        Bruker? record = await userRepository.GetBy(record => record.Id == userId);
        return record is null
            ? Response<UserDto>.Error(UserNotFoundError)
            : Response<UserDto>.Ok(MapUser(record));
    }

    public async Task<Response> Update(Guid id, EditUserDto dto)
    {
        Bruker? record = await userRepository.GetBy(record => record.Id == id);
        if (record is null)
        {
            return Response.Error(UserNotFoundError);
        }

        record.Fornavn = dto.FirstName;
        record.Etternavn = dto.LastName;
        record.Epost = dto.Email;
        record.IsAdmin = dto.IsAdmin;
        record.Aktiv = dto.IsActive;
        
        return await userRepository.Update(record);
    }

    public async Task<Response> ToggleActive(Guid userId)
    {
        Bruker? record = await userRepository.GetBy(record => record.Id == userId);
        if (record is null)
        {
            return Response.Error(UserNotFoundError);
        }
        
        record.Aktiv = !record.Aktiv;
        
        return await userRepository.Update(record);
    }

    private UserDto MapUser(Bruker model) => new UserDto()
    {
        Id = model.Id,
        FirstName = model.Fornavn,
        LastName = model.Etternavn,
        Email = model.Epost,
        IsAdmin = model.IsAdmin,
        IsActive = model.Aktiv
    };
}