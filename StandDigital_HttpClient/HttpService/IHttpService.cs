using ErrorOr;
using static StandDigital_HttpClient.HttpService.HttpServiceRequester;

namespace StandDigital_HttpClient.HttpService;

public interface IHttpService
{
    Task<ErrorOr<Tout?>> SendAsync<Tout>(
            ServicesEndpoints service,
            Dictionary<string, string>? queryStringParams,
            HashSet<string>? parameters,
            CancellationToken cancellationToken);
}
