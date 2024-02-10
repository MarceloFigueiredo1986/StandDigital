using StandDigital_Shared.Entities;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public interface IBrandService
{
    Task<BrandEntity[]> GetAllBrandsAsync(CancellationToken cancellationToken);
}
