using ErrorOr;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Web;

namespace StandDigital_HttpClient.HttpService;

public sealed class HttpServiceRequester(HttpClient httpClientFactory, IConfiguration configuration) : IHttpService
{
    private readonly HttpClient _httpClientFactory;
    private readonly IConfiguration _configuration;

    public enum ServicesEndpoints
    {
        ValidateLogin,
        CreateUser,
        AllBrands,
        AllExtras,
        BrandModels
    }

    string GetMicroserviceEndpoint(ServicesEndpoints endpoint)
        => endpoint switch
        {
            ServicesEndpoints.AllBrands => Path.Combine(
                configuration.GetValue<string>("BrandMicroservice") ?? throw new Exception("Brand Microservice settings is empty"),
                configuration.GetValue<string>("AllBrandEndpoint") ?? throw new Exception("AllBrandEndpoint settings empty")),
            ServicesEndpoints.AllExtras => Path.Combine(
                configuration.GetValue<string>("BrandMicroservice") ?? throw new Exception("Brand Microservice settings is empty"),
                configuration.GetValue<string>("ExtrasModelsEndpoint") ?? throw new Exception("ExtrasModelsEndpoint settings empty")),
            ServicesEndpoints.BrandModels => Path.Combine(
                configuration.GetValue<string>("BrandMicroservice") ?? throw new Exception("Brand Microservice settings is empty"),
                configuration.GetValue<string>("BrandModelsEndpoint") ?? throw new Exception("BrandModelsEndpoint settings empty")),
            _ => throw new Exception("Endpoint not found")
        };

    public async Task<ErrorOr<Tout?>> SendAsync<Tout>(
        ServicesEndpoints service,
        Dictionary<string, string>? queryStringParams, 
        HashSet<string>? parameters,
        CancellationToken cancellationToken)
    {
        var endpointUrl = GetMicroserviceEndpoint(service);  

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
