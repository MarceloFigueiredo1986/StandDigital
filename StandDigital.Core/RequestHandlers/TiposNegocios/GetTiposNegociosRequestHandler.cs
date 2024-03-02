using MediatR;
using StandDigital.Core.Database.Repositories.TiposNegocios;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.TiposNegocios;

internal class GetTiposNegociosRequestHandler(ITiposNegociosRepository tiposNegociosRepository) : IRequestHandler<GetTiposNegociosRequest, IEnumerable<TipoNegocioEntity>>
{
    private readonly ITiposNegociosRepository _tiposNegociosRepository = tiposNegociosRepository;

    public async Task<IEnumerable<TipoNegocioEntity>> Handle(GetTiposNegociosRequest request, CancellationToken cancellationToken)
        => await _tiposNegociosRepository.GetTiposNegociosAsync(cancellationToken);
}
