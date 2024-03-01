using StandDigital.Entities;
using StandDigital_Shared.Persistence;

namespace StandDigital.Services.Repositories.TiposNegocios;

public sealed class TiposNegociosRepository(IDatabase database) : ITiposNegociosRepository
{
    private readonly IDatabase _dbcontext = database;

    public async Task<IEnumerable<TipoNegocioEntity>> GetTiposNegociosAsync(CancellationToken cancellationToken)
     => await _dbcontext.GetAsync<TipoNegocioEntity>("""
            Select * from TipoNegocios
            """);
}
