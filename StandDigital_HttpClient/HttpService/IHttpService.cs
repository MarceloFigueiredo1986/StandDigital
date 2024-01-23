namespace StandDigital_HttpClient.HttpService;

public interface IHttpService
{
    Task<HttpResponseMessage> SendAsync(
            string endpointUrl,
            Dictionary<string, string>? queryStringParams,
            Dictionary<string, string>? parameters,
            CancellationToken cancellationToken);
}
