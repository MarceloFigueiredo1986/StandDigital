using StandDigital_Shared.Models;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public interface IBrandService
{
    Task<BrandEntity[]> GetAllBrandsAsync(CancellationToken cancellationToken);
}
