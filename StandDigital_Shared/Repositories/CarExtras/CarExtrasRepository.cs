using ErrorOr;
using StandDigital_Shared.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital_Shared.Repositories.CarExtras;

public sealed class CarExtrasRepository(IDatabase database) : ICarExtrasRepository
{
    private readonly IDatabase _dbContext = database;

    public Task<ErrorOr<int>> CreateAsync(ExtraEntity value, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public async Task<IEnumerable<ExtraEntity>> GetAllAsync()
        => await _dbContext.GetAsync<ExtraEntity>("""
            Select * from Extras
            """);
}
