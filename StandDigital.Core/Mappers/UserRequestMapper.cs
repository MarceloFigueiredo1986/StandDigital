using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.User;

namespace StandDigital.Core.Mappers;

public static class UserRequestMapper
{
    public static UserEntity ToUserEntity(this UserRequest userRequest)
        => new UserEntity
        {
            Address = userRequest.Address!,
            Email = userRequest.Email!,
            Ativo = 1,
            Mobile = userRequest.Mobile!.ToString()!,
            Name = userRequest.Name!,
            NIF = userRequest.NIF!,
            Password = userRequest.Password!,
            Username = userRequest.Username!
        };
}
