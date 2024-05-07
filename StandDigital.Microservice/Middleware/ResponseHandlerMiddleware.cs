
namespace StandDigital.Microservice.Middleware;

public class ResponseHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next.Invoke(context);

        // Verifique se o status code é 404 e altere-o
        if (context.Response.StatusCode == 404)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Página não encontrada, mas o status foi alterado para 200");
        }
    }
}
