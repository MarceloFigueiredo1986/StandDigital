using Microsoft.Extensions.Caching.Memory;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Repositories.CarBrandModels;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public class BrandModelService(
    IMemoryCache memoryCache,
    ICarBrandModelRepository carBrandModelRepository) : IBrandModelService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ICarBrandModelRepository _carBrandModelRepository = carBrandModelRepository;
    private const string CacheKey = "BrandModels";

    public async Task<BrandModelEntity[]> GetModelsByBrandAsync(int brandId)
    {
        if (!_memoryCache.TryGetValue(CacheKey, out BrandModelEntity[] brandModels))
        {
            var dbBrandModels = await _carBrandModelRepository.GetAllAsync();
            brandModels = dbBrandModels.ToArray();

            if (brandModels is null || !brandModels.Any())
                return Array.Empty<BrandModelEntity>();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(20));

            _memoryCache.Set(CacheKey, brandModels, cacheEntryOptions);
        }

        return brandModels!.Where(a => a.MarcaId == brandId).ToArray();
    }
}
