using ErrorOr;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Web;

namespace StandDigital_HttpClient.HttpService;

public sealed class HttpServiceRequester(HttpClient httpClientFactory, IConfiguration configuration) : IHttpService
{
    public enum ServicesEndpoints
    {
        ValidateLogin,
        CreateUser,
        AllBrands,
        AllExtras,
        BrandModels,
        NewCarRequest,
        NewBid,
        UpdateCarDeal,
        AllDealTypes,
        DealDetails,
        HighlightedDeals,
        OpenDealsByUser,
        DealPayment,
        CompleteCarById,
        ClosedDealsByUser,
        DeleteDeal
    }

    string BrandMicroservice => configuration.GetValue<string>("Endpoints:BrandMicroservice") ?? throw new Exception("Brand Microservice settings is empty");
    string MainMicroservice => configuration.GetValue<string>("Endpoints:MainMicroservice") ?? throw new Exception("Main Microservice settings is empty");

    string GetMicroserviceEndpoint(ServicesEndpoints endpoint)
        => endpoint switch
        {
            ServicesEndpoints.AllBrands => Path.Combine(
                BrandMicroservice,
                configuration.GetValue<string>("Endpoints:AllBrands") ?? throw new Exception("AllBrandEndpoint settings empty")),
            ServicesEndpoints.AllExtras => Path.Combine(
                BrandMicroservice,
                configuration.GetValue<string>("Endpoints:AllExtras") ?? throw new Exception("ExtrasModelsEndpoint settings empty")),
            ServicesEndpoints.BrandModels => Path.Combine(
                BrandMicroservice,
                configuration.GetValue<string>("Endpoints:BrandById") ?? throw new Exception("BrandModelsEndpoint settings empty")),
            ServicesEndpoints.ValidateLogin => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:ValidateLogin") ?? throw new Exception("ValidateLoginEndpoint settings empty")),
            ServicesEndpoints.CreateUser => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:CreateUser") ?? throw new Exception("CreateUserEndpoint settings empty")),
            ServicesEndpoints.NewCarRequest => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:NewCar") ?? throw new Exception("NewCarEndpoint settings empty")),
            ServicesEndpoints.NewBid => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:NewBid") ?? throw new Exception("NewBidDetailsEndpoint settings empty")),
            ServicesEndpoints.AllDealTypes => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:AllDealTypes") ?? throw new Exception("AllDealTypesEndpoint settings empty")),
            ServicesEndpoints.DealDetails => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:DealDetails") ?? throw new Exception("DealDetailsEndpoint settings empty")),
            ServicesEndpoints.HighlightedDeals => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:HighlightedDeals") ?? throw new Exception("HighlightedDealsEndpoint settings empty")),
            ServicesEndpoints.OpenDealsByUser => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:OpenDealsByUser") ?? throw new Exception("OpenDealsByUserEndpoint settings empty")),
            ServicesEndpoints.DealPayment => Path.Combine(
                 MainMicroservice,
                 configuration.GetValue<string>("Endpoints:DealPayment") ?? throw new Exception("DealPaymentEndpoint settings empty")),
            ServicesEndpoints.CompleteCarById => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:CompleteCarById") ?? throw new Exception("CompleteCarByIdEndpoint settings empty")),
            ServicesEndpoints.ClosedDealsByUser => Path.Combine(
                 MainMicroservice,
                 configuration.GetValue<string>("Endpoints:ClosedDealsByUser") ?? throw new Exception("ClosedDealsByUserEndpoint settings empty")),
            ServicesEndpoints.DeleteDeal => Path.Combine(
                MainMicroservice,
                configuration.GetValue<string>("Endpoints:DeleteDeal") ?? throw new Exception("DeleteDealEndpoint settings empty")),
            _ => throw new Exception("Endpoint not found")
        };

    public async Task<ErrorOr<Tout?>> SendAsync<Tout>(
        HttpMethod method,
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

        var request = new HttpRequestMessage(
            method: method,
            requestUri: endpointUrl);

        var result = await httpClientFactory.SendAsync(request, cancellationToken);

        if (!result.IsSuccessStatusCode)
            return Error.Failure(description: await result.Content.ReadAsStringAsync());

        if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            return default(Tout);

        var resulString = await result.Content.ReadAsStringAsync();
        var resultJson = JsonConvert.DeserializeObject<Tout>(resulString);

        return resultJson;
    }

    public async Task<ErrorOr<Tout?>> SendAsync<Tout, Tbody>(
        HttpMethod method,
        ServicesEndpoints service, 
        Dictionary<string, string>? queryStringParams, 
        HashSet<string>? parameters, 
        Tbody bodyObj,
       CancellationToken cancellationToken)
    {
        var endpointUrl = GetMicroserviceEndpoint(service);

        if (parameters is not null)
            endpointUrl += string.Join('/', parameters);

        if (queryStringParams is not null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, string value) in queryStringParams)
                query[key] = value;

            endpointUrl = string.Join("?", endpointUrl, query.ToString());
        }

        var request = new HttpRequestMessage(
                method: method,
                requestUri: endpointUrl);

        request.Content = JsonContent.Create(bodyObj);

        var result = await httpClientFactory.SendAsync(
            request : request,
            cancellationToken: cancellationToken);

        if (!result.IsSuccessStatusCode)
            return Error.Failure(description: await result.Content.ReadAsStringAsync());

        var resulString = await result.Content.ReadAsStringAsync();
        var resultJson = JsonConvert.DeserializeObject<Tout>(resulString);

        return resultJson;
    }
}
