using Microsoft.Extensions.Caching.Memory;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Repositories.CarExtras;

namespace StandDigital_SharedMicroservice.EndpointsServices;

public class ExtrasService(
    IMemoryCache memoryCache,
    ICarExtrasRepository carExtrasRepository) : IExtrasService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
private readonly ICarExtrasRepository _carExtrasRepository = carExtrasRepository;

private const string CacheKey = "Extras";
public async Task<ExtraEntity[]> GetAllExtrasAsync(CancellationToken cancellationToken)
    {
        if (!_memoryCache.TryGetValue(CacheKey, out ExtraEntity[] extras))
        {
            var dbExtras = await _carExtrasRepository.GetAllAsync();
            extras = dbExtras.ToArray();

            if (extras is null || !extras.Any())
                return Array.Empty<ExtraEntity>();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(20));

            _memoryCache.Set(CacheKey, extras, cacheEntryOptions);
        }

        return extras;
    }
}
