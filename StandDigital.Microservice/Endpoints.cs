using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StandDigital.Core.Aggregates.CarAdvertisement;
using StandDigital.Core.Mappers;
using StandDigital.Core.RequestHandlers.CarDealDetails;
using StandDigital.Core.RequestHandlers.DealBiding;
using StandDigital.Core.RequestHandlers.DealPayment;
using StandDigital.Core.RequestHandlers.DeleteAdvertisement;
using StandDigital.Core.RequestHandlers.HighlightedDeals;
using StandDigital.Core.RequestHandlers.NewCarDeal;
using StandDigital.Core.RequestHandlers.OpenDeals;
using StandDigital.Core.RequestHandlers.TiposNegocios;
using StandDigital.Core.RequestHandlers.User;
using StandDigital.Core.RequestHandlers.UserClosedDeals;
using StandDigital.Core.RequestHandlers.UserOpenDeals;
using StandDigital.Core.RequestHandlers.ValidateLogin;
using StandDigital.Microservice.BodyModels;
using System.Buffers.Text;
using System.Text;

namespace StandDigital.Microservice;

public static class Endpoints
{
    public static void EndpointsInstaller(this WebApplication app)
    {
        app.MapGet("/user/v1/validatelogin/{user}/{pass}", async (IMediator mediator, CancellationToken cancellationToken,
            string user,
            string pass) => 
        {
            var result = await mediator.Send(new ValidateLoginRequest(
                Encoding.UTF8.GetString(Convert.FromBase64String(user)),
                Encoding.UTF8.GetString(Convert.FromBase64String(pass))),
                cancellationToken);
            return result is not null ? Results.Ok(result) : Results.NoContent();
        });
        app.MapPost("/newuser/v1", async (
            [FromBody] CreateUserRequest car, 
            IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(car, cancellationToken);
            return result;
        });
        app.MapPost("/newcar/v1", async (
            [FromBody] CarAdvertisementAggregate car,
            IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new NewCarDealRequest(car), cancellationToken);
            return result;
        });
        app.MapPost("/newbid/v1/{carroId}", async (
            [FromBody] NewBidModel body,
            int carroId,
            IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new AuctionRequest(carroId,body.NewBid,body.SessionUserId), 
                cancellationToken);
            return result;
        });
        app.MapGet("/deals/v1/types", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetTiposNegociosRequest(), cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });
        app.MapGet("/deal/v1/{dealId}", async (IMediator mediator, CancellationToken cancellationToken,
            [FromRoute] int dealId) =>
        {
            var result = await mediator.Send(new CarDealDetailsRequest(dealId), cancellationToken);
            return result;
        });
        app.MapGet("/deals/v1/highlighted", async (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetHighlightedDealsQuery(), cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });
        app.MapGet("/deals/v1/user/{userId}", async (IMediator mediator, CancellationToken cancellationToken,
            [FromRoute] int userId) =>
        {
            var result = await mediator.Send(new UserOpenDealsRequest(userId), cancellationToken);
            return result is not null ? Results.Ok(result) : Results.NoContent();
        });
        app.MapGet("/deal/v1/payment/{carroId}", async (IMediator mediator, CancellationToken cancellationToken,
            [FromRoute] int carroId) =>
        {
            var result = await mediator.Send(new DealPaymentQuery(carroId), cancellationToken);
            return result;
        });
        app.MapGet("/car/v1/details/{carroId}", async (IMediator mediator, CancellationToken cancellationToken,
            [FromRoute] int carroId) =>
        {
            var result = await mediator.Send(new CarDealDetailsRequest(carroId), cancellationToken);
            return result;
        });
        app.MapGet("/deals/v1/closed/user/{userId}", async (IMediator mediator, CancellationToken cancellationToken,
            [FromRoute] int userId) =>
        {
            var result = await mediator.Send(new UserClosedDealsRequest(userId), cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });
        app.MapDelete("/deal/v1/delete/{carroId}", async (IMediator mediator, CancellationToken cancellationToken,
            [FromRoute] int carroId) =>
        {
            var result = await mediator.Send(new DeleteAdvertisementRequest(carroId), cancellationToken);
            return result;
        });
    }
}
