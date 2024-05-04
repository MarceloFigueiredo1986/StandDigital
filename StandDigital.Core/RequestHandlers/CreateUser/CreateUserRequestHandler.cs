using ErrorOr;
using MediatR;
using StandDigital.Core.Database.Repositories.User;
using StandDigital.Core.Mappers;

namespace StandDigital.Core.RequestHandlers.User;

public sealed class CreateUserRequestHandler(IUserRepository userRepository) : IRequestHandler<CreateUserRequest, ErrorOr<bool>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ErrorOr<bool>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
		try
		{
            var changedRecords = await _userRepository.CreateAsync(request.ToUserEntity(), cancellationToken);
            if (changedRecords.IsError)
                return changedRecords.FirstError;

            return true;
        }
		catch (Exception ex)
		{
			return Error.Failure(description: ex.Message);
		}
    }
}
