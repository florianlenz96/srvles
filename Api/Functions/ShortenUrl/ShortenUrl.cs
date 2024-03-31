using System.Net;
using System.Text.Json;
using Azure.Data.Tables;
using BlazorApp.Shared.Models.ShortenedUrl;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;

namespace Api;

public class ShortenUrl
{
    private readonly string svrlesBackendHost;
    private readonly string backendFunctionKey;
    private readonly string backendFunctionHost;
    private readonly HttpClient httpClient;

    public ShortenUrl(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
        httpClient = clientFactory.CreateClient();
        svrlesBackendHost = configuration["SvrlesBackendHost"];
        backendFunctionKey = configuration["BackendFunctionKey"];
        backendFunctionHost = configuration["BackendFunctionHost"];
    }
    
    [Function("ShortenUrl")]
    public async Task<ShortenUrlResponse> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var request = JsonSerializer.Deserialize<ShortenUrlRequest>(req.Body);

        var response = await this.httpClient.PostAsync(new Uri(backendFunctionHost + "api/ShortenUrl?code=" + this.backendFunctionKey),
            new StringContent(JsonSerializer.Serialize(request)));
        var shortenId = await response.Content.ReadAsStringAsync();
        
        var responseMessage = req.CreateResponse(HttpStatusCode.OK);
        responseMessage.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await responseMessage.WriteStringAsync($"{this.svrlesBackendHost}/{shortenId}");

        return new ShortenUrlResponse
        {
            Response = responseMessage,
        };
    }
}