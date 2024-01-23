using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
using StandDigital_HttpClient.HttpService;

namespace StandDigital_HttpClient;

public static class Installer
{
    public static IServiceCollection InstallHttpClient(this IServiceCollection services)
    {
        services.
            AddHttpClient<IHttpService,HttpServiceRequester>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
            .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                        retryAttempt)));
    }
}
