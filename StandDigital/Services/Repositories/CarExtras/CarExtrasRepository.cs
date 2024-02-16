using ErrorOr;
using StandDigital.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital.Services.Repositories.CarExtras;

public sealed class CarExtrasRepository(IDatabase dbContext) : ICarExtrasRepository
{
    private readonly IDatabase _dbContext = dbContext;

    public async Task<ErrorOr<int>> CreateAsync(CarExtraEntity value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarExtraId = await _dbContext.InsertAsyncAndGetIdAsync<CarExtraEntity, int>(
                     """
            INSERT INTO 
            CarrosExtras
            VALUES
            (
            @CarroId,
            )
            """, value, cancellationToken);

            if (createdCarExtraId == 0)
                throw new Exception($"{nameof(CarExtraEntity)} not created");

            return createdCarExtraId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<CarExtraEntity>> GetAllAsync()
     => await _dbContext.GetAsync<CarExtraEntity>("""
            Select * from CarrosExtras
            """);
}
