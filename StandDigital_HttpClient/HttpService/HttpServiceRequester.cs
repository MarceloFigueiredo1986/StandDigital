using ErrorOr;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace StandDigital_HttpClient.HttpService;

public sealed class HttpServiceRequester : IHttpService
{
    private readonly HttpClient _httpClientFactory;
    private readonly IConfiguration _configuration;

    public HttpServiceRequester(HttpClient httpClientFactory, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _httpClientFactory.BaseAddress = new Uri(_configuration.GetValue<string>("BrandMicroservice") 
            ?? throw new NullReferenceException("BrandMicroservice"));
    }

    public async Task<ErrorOr<Tout?>> SendAsync<Tout>(
        string endpointUrl,
        Dictionary<string, string>? queryStringParams, 
        HashSet<string>? parameters,
        CancellationToken cancellationToken)
    {
        if (parameters is not null)
           endpointUrl += string.Join('/', parameters);

        if(queryStringParams is not null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, string value) in queryStringParams)
                query[key] = value;

           endpointUrl = string.Join("?", endpointUrl, query.ToString());
        }

        var result = await _httpClientFactory.GetAsync(endpointUrl,cancellationToken);

        if (!result.IsSuccessStatusCode)
            return Error.Failure(description: await result.Content.ReadAsStringAsync());

        var resulString = await result.Content.ReadAsStringAsync();
        var resultJson = JsonConvert.DeserializeObject<ResultDeserializer<Tout>>(resulString);

        return resultJson!.Result;
    }
}

class ResultDeserializer<T>
{
    public T Result { get; set; }
}
