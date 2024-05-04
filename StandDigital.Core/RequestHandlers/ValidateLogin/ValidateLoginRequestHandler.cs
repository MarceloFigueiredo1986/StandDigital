using MediatR;
using StandDigital.Core.Database.Repositories.User;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.ValidateLogin;

internal class ValidateLoginRequestHandler(IUserRepository userRepository) : IRequestHandler<ValidateLoginRequest, UserEntity?>
{
    public async Task<UserEntity?> Handle(ValidateLoginRequest request, CancellationToken cancellationToken)
        => await userRepository.ValidateLogin(request.username, request.password);
}
