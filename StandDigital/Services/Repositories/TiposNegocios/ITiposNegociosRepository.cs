using StandDigital.Entities;

namespace StandDigital.Services.Repositories.TiposNegocios;

public interface ITiposNegociosRepository
{
    Task<IEnumerable<TipoNegocioEntity>> GetTiposNegociosAsync(CancellationToken cancellationToken);
}
