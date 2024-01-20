using ErrorOr;
using StandDigital.Entities;
using StandDigital.Persistence;

namespace StandDigital.Repositories.CarBrands;

public sealed class CarBrandRepository(IDatabase dbContext) : ICarBrandRepository
{
    private readonly IDatabase _dbContext = dbContext;

    public async Task<ErrorOr<int>> CreateAsync(Brand value, CancellationToken cancellationToken)
    {
        try
        {
            var createdBrandId = await _dbContext.InsertAsyncAndGetIdAsync<Brand, int>(
                     """
            INSERT INTO 
            Marcas
            VALUES
            (
            @Nome,
            )
            """, value, cancellationToken);

            if (createdBrandId == 0)
                throw new Exception($"{nameof(Brand)} not created");

            return createdBrandId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<Brand>> GetAllAsync()
     => await _dbContext.GetAsync<Brand>("""
            Select * from Marcas
            """);
}
