using MediatR;
using StandDigital.Core.RequestHandlers.ValidateLogin;

namespace StandDigital.Microservice;

public static class Endpoints
{
    public static void EndpointsInstaller(this WebApplication app)
    {
        app.MapGet("/user/v1/validatelogin/{user}/{pass}", async (IMediator mediator, CancellationToken cancellationToken, string user, string pass) => 
        {
            var result = await mediator.Send(new ValidateLoginRequest(user, pass), cancellationToken);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
