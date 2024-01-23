using ErrorOr;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital_Shared.Repositories.CarBrandModels;

public sealed class CarBrandModelRepository(IDatabase dbContext) : ICarBrandModelRepository
{
    private readonly IDatabase _dbContext = dbContext;

    public async Task<ErrorOr<int>> CreateAsync(BrandModelEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdBrandModelEntityId = await _dbContext.InsertAsyncAndGetIdAsync<BrandModelEntity, int>(
                     """
            INSERT INTO 
            Modelos
            VALUES
            (
            @Nome,
            @Cilindrada,
            @Cavalagem,
            @MarcaId
            )
            """, value, cancellationToken);

            if (createdBrandModelEntityId == 0)
                throw new Exception($"{nameof(BrandModelEntity)} not created");


            return createdBrandModelEntityId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<BrandModelEntity>> GetAllAsync()
     => await _dbContext.GetAsync<BrandModelEntity>("""
            Select * from Modelos
            """);
}
