using MediatR;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.ValidateLogin;

public record ValidateLoginRequest(string username, string password) : IRequest<UserEntity?>;
