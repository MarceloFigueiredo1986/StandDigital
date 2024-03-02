
using StandDigital.Core.Entities;

namespace StandDigital.Core.Database.Repositories.User;

internal interface IUserRepository
{
    Task<UserEntity?> ValidateLogin(string user, string password);
}
