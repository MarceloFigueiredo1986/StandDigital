using MediatR;
using StandDigital.Core.Database.Repositories.User;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.ValidateLogin;

internal class ValidateLoginRequestHandler(IUserRepository userRepository) : IRequestHandler<ValidateLoginRequest, UserEntity?>
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task<UserEntity?> Handle(ValidateLoginRequest request, CancellationToken cancellationToken)
        => _userRepository.ValidateLogin(request.username, request.password);
}
