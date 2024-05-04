using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StandDigital.Microservice.Middleware;

public static class ExceptionHandlerMiddleware
{
    public static IServiceCollection InstallExceptionMiddleware(this IServiceCollection services)
    {
        services.AddProblemDetails(config =>
        {
            config.CustomizeProblemDetails = opt =>
            {
                opt.ProblemDetails = new ProblemDetails()
                {
                    Detail = opt.Exception.Message,
                    Status = GetStatusCode(opt.Exception),
                };
            };
        });

        return services;

        int GetStatusCode(Exception exception)
            => exception.GetType().Name switch
            {
                nameof(ValidationException) or nameof(ArgumentNullException) => StatusCodes.Status400BadRequest,
                nameof(Exception) => StatusCodes.Status500InternalServerError,
                nameof(AggregateException) => GetStatusCode(exception.InnerException!),
                _ => StatusCodes.Status500InternalServerError
            };

    }
}
