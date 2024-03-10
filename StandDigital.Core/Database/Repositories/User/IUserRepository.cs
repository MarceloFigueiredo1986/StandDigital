
using StandDigital.Core.Entities;
using StandDigital_Shared.Repositories;

namespace StandDigital.Core.Database.Repositories.User;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> ValidateLogin(string user, string password);
}
