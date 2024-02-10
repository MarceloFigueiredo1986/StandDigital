using StandDigital_Shared.Entities;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public interface IExtrasService
{
    Task<ExtraEntity[]> GetAllExtrasAsync(CancellationToken cancellationToken);
}
