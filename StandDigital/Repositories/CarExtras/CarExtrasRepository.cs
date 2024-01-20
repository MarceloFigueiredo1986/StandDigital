using ErrorOr;
using StandDigital.Entities;
using StandDigital.Persistence;

namespace StandDigital.Repositories.CarExtras;

public sealed class CarExtrasRepository(IDatabase dbContext) : ICarExtrasRepository
{
    private readonly IDatabase _dbContext = dbContext;

    public async Task<ErrorOr<int>> CreateAsync(CarExtra value, CancellationToken cancellationToken)
    {
        try
        {
            var createdCarExtraId = await _dbContext.InsertAsyncAndGetIdAsync<CarExtra, int>(
                     """
            INSERT INTO 
            CarrosExtras
            VALUES
            (
            @CarroId,
            )
            """, value, cancellationToken);

            if (createdCarExtraId == 0)
                throw new Exception($"{nameof(CarExtra)} not created");

            return createdCarExtraId;
        }
        catch (Exception ex)
        {
            return Error.Failure($"{ex}");
        }
    }

    public async Task<IEnumerable<CarExtra>> GetAllAsync()
     => await _dbContext.GetAsync<CarExtra>("""
            Select * from CarrosExtras
            """);
}
