using Microsoft.Extensions.Caching.Memory;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Repositories.CarBrands;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public sealed class BrandService(
    IMemoryCache memoryCache,
    ICarBrandRepository carBrandRepository) : IBrandService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ICarBrandRepository _carBrandRepository = carBrandRepository;

    private const string CacheKey = "Brands";

    public async Task<BrandEntity[]> GetAllBrandsAsync(CancellationToken cancellationToken)
    {
        if (!_memoryCache.TryGetValue(CacheKey, out BrandEntity[] brands))
        {
            var dbBrands = await _carBrandRepository.GetAllAsync();
            brands = dbBrands.ToArray();

            if (brands is null || !brands.Any())
                return Array.Empty<BrandEntity>();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(20));

            _memoryCache.Set(CacheKey, brands, cacheEntryOptions);
        }

        return brands;
    }
}
