using StandDigital_SharedMicroservice.EndpointsServices;

namespace StandDigital_SharedMicroservice;

public static class Endpoints
{
    public static void EndpointsInstaller(this WebApplication app)
    {
        app.MapGet("/brands/v1/all", httpconxt => DealWithRequest(httpconxt, async input =>
        {
            var service = input.ServiceProvider.GetRequiredService<IBrandService>();
            return await service.GetAllBrandsAsync(httpconxt.RequestAborted);
        }));
        app.MapGet("/brandmodels/v1/byBrandid/{brandid}", httpconxt => DealWithRequest(httpconxt, async input =>
        {
            var brandIdint = Convert.ToInt32(httpconxt.Request.RouteValues["brandid"]);
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(brandIdint));

            var service = input.ServiceProvider.GetRequiredService<IBrandModelService>();
            return await service.GetModelsByBrandAsync(brandIdint);
        }));

        app.MapGet("/allcarextrastypes/v1/", httpconxt => DealWithRequest(httpconxt, async input =>
        {
            var service = input.ServiceProvider.GetRequiredService<IExtrasService>();
            return await service.GetAllExtrasAsync(httpconxt.RequestAborted);
        }));
    }

    private static async Task DealWithRequest<Tout>(HttpContext httpContext, Func<IServiceScope, Tout> endpointServiceHandler)
    {
        var scope = httpContext.RequestServices.CreateScope();
        var result = endpointServiceHandler(scope);
        await httpContext.Response.WriteAsJsonAsync(result, httpContext.RequestAborted);
    }
}
