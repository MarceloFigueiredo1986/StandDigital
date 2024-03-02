
using StandDigital.Core.Entities;

namespace StandDigital.Core.Database.Repositories.TiposNegocios;

internal interface ITiposNegociosRepository
{
    Task<IEnumerable<TipoNegocioEntity>> GetTiposNegociosAsync(CancellationToken cancellationToken);
}
