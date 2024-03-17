using System.Net;
using System.Text.Json;
using Api.Services;
using BlazorApp.Shared.Models.ShortenedUrl;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Api;

public class ShortenUrl
{
    private readonly SvrlesClient _svrlesClient;

    public ShortenUrl(SvrlesClient svrlesClient)
    {
        _svrlesClient = svrlesClient;
    }

    [Function("ShortenUrl")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var request = JsonSerializer.Deserialize<ShortenUrlRequest>(req.Body);
        
        var response = await _svrlesClient.ShortenUrl(request.Url);
        
        var responseJson = JsonSerializer.Serialize(new ShortenUrlResponse {ShortenedUrl = response});
        var responseMessage = req.CreateResponse(HttpStatusCode.OK);
        responseMessage.Headers.Add("Content-Type", "application/json");
        await responseMessage.WriteStringAsync(responseJson);
        return responseMessage;
    }
}