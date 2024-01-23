using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StandDigital_HttpClient.HttpService;

public sealed class HttpServiceRequester(IHttpClientFactory httpClientFactory) : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<HttpResponseMessage> SendAsync(
        string endpointUrl,
        Dictionary<string, string>? queryStringParams, 
        Dictionary<string, string>? parameters,
        CancellationToken cancellationToken)
    {
        var requester = _httpClientFactory.CreateClient();

        if (parameters is not null)
            foreach ((string key, string value) in parameters)
                requester.DefaultRequestHeaders.Add(key, value);

        if(queryStringParams is not null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, string value) in queryStringParams)
                query[key] = value;

           endpointUrl = string.Join("?", endpointUrl, query.ToString());
        }

        return await requester.GetAsync(endpointUrl,cancellationToken);
    }
}
