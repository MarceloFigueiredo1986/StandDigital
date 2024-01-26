using ErrorOr;

namespace StandDigital_HttpClient.HttpService;

public interface IHttpService
{
    Task<ErrorOr<Tout?>> SendAsync<Tout>(
            string endpointUrl,
            Dictionary<string, string>? queryStringParams,
            HashSet<string>? parameters,
            CancellationToken cancellationToken);
}
