using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using StandDigital.Core.Database.Repositories.User;
using StandDigital.Core.Entities;
using StandDigital.Core.RequestHandlers.ValidateLogin;

namespace StandDigital.Microservice;

public static class Endpoints
{
    public static void EndpointsInstaller(this WebApplication app)
    {
        app.MapGet("/user/v1/validatelogin/{user}/{pass}", async (IMediator mediator, CancellationToken cancellationToken, string user, string pass) => 
        {
            var result = await DealWithRequest(mediator, new ValidateLoginRequest(user, pass), cancellationToken);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }

    private static async Task<Tout> DealWithRequest<Tout>(
        IMediator mediator,
        IRequest<Tout> mediatorModel,
        CancellationToken cancellationToken)
        => await mediator.Send(mediatorModel, cancellationToken);
}
