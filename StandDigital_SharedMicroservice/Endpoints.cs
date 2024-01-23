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
        app.MapGet("/brandmodels/v1/byBrandidAndFuel/{brandid}/{fuel}", httpconxt => DealWithRequest(httpconxt, async input =>
        {
            var brandIdint = Convert.ToInt32(httpconxt.Request.RouteValues["brandid"]);
            var fuel = httpconxt.Request.RouteValues["fuel"]?.ToString();
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(brandIdint));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(fuel));

            var service = input.ServiceProvider.GetRequiredService<IBrandModelService>();
            return await service.GetModelsByBrandAndFuelAsync(brandIdint,fuel!);
        }));
    }

    private static async Task DealWithRequest<Tout>(HttpContext httpContext, Func<IServiceScope, Tout> endpointServiceHandler)
    {
        var scope = httpContext.RequestServices.CreateScope();
        var result = endpointServiceHandler(scope);
        await httpContext.Response.WriteAsJsonAsync(result, httpContext.RequestAborted);
    }
}
