using StandDigital.Entities;

namespace StandDigital.Services.Repositories.User;

public interface IUserRepository
{
    Task<UserEntity?> ValidateLogin(string user, string password);
}
