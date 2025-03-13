namespace SeaEco.Services.UserServices;

public interface ICurrentUserContext
{
    Guid Id { get; }
    string Email { get; }
}