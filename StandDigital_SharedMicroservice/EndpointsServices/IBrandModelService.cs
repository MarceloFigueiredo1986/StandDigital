using StandDigital_Shared.Entities;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public interface IBrandModelService
{
    Task<BrandModelEntity[]> GetModelsByBrandAsync(int brandId);
}
