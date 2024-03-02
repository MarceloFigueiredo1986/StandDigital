using MediatR;
using StandDigital.Core.Entities;

namespace StandDigital.Core.RequestHandlers.TiposNegocios;

public sealed record GetTiposNegociosRequest() : IRequest<IEnumerable<TipoNegocioEntity>>;