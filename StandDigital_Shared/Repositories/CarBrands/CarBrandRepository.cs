using ErrorOr;
using StandDigital_Shared.Models;
using StandDigital_Shared.Persistence;

namespace StandDigital_Shared.Repositories.CarBrands;

public sealed class CarBrandRepository(IDatabase dbContext) : ICarBrandRepository
{
    private readonly IDatabase _dbContext = dbContext;

    public async Task<ErrorOr<int>> CreateAsync(BrandEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdBrandId = await _dbContext.InsertAsyncAndGetIdAsync<BrandEntity, int>(
                     """
            INSERT INTO 
            Marcas
            VALUES
            (
            @Nome,
            )
            """, value, cancellationToken);

            if (createdBrandId == 0)
                throw new Exception($"{nameof(BrandEntity)} not created");

            return createdBrandId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<BrandEntity>> GetAllAsync()
     => await _dbContext.GetAsync<BrandEntity>("""
            Select * from Marcas
            """);
}
