using ErrorOr;
using StandDigital.Core.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital.Core.Database.Repositories.CarExtras;

internal sealed class CarExtrasRepository(IDatabase dbContext) : ICarExtrasRepository
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
            (CarroId,
            Extra)
            VALUES
            (
            @CarroId,
            @Extra
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

    public async Task<IEnumerable<string>> GetAllExtrasByCarAsync(int carId, CancellationToken cancellationToken)
     => await _dbContext.GetAsync<string>("""
            Select Extra from CarrosExtras where CarroId = @carId
            """, new Dictionary<string, object> { { "@carId", carId } });
}
