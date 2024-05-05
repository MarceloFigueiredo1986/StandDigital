using ErrorOr;
using static StandDigital_HttpClient.HttpService.HttpServiceRequester;

namespace StandDigital_HttpClient.HttpService;

public interface IHttpService
{
    Task<ErrorOr<Tout?>> SendAsync<Tout>(
            HttpMethod method,
            ServicesEndpoints service,
            Dictionary<string, string>? queryStringParams,
            HashSet<string>? parameters,
            CancellationToken cancellationToken);

    Task<ErrorOr<Tout?>> SendAsync<Tout, Tbody>(
        HttpMethod method,
        ServicesEndpoints service,
        Dictionary<string, string>? queryStringParams,
        HashSet<string>? parameters,
        Tbody bodyObj,
       CancellationToken cancellationToken);
}
