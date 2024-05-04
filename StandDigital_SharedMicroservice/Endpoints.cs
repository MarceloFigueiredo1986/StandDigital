using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using StandDigital_SharedMicroservice.EndpointsServices;
using System.Threading;

namespace StandDigital_SharedMicroservice;

public static class Endpoints
{
    public static void EndpointsInstaller(this WebApplication app)
    {
        app.MapGet("/brands/v1/all", async(IBrandService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetAllBrandsAsync(cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });
        app.MapGet("/brandmodels/v1/byBrandid/{brandId}", async (IBrandModelService service, CancellationToken cancellationToken, int brandId) =>
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(brandId));

            var result = await service.GetModelsByBrandAsync(brandId);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
        app.MapGet("/extras/v1/all", async(IExtrasService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetAllExtrasAsync(cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });
    }
}
