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
    private readonly string SvrlesBackendHost;

    public ShortenUrl(IConfiguration configuration)
    {
        SvrlesBackendHost = configuration["SvrlesBackendHost"].ToString();
    }
    
    [Function("ShortenUrl")]
    public async Task<ShortenUrlResponse> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        [TableInput("ShortUrlTable", Connection = "SvrlesDbConnectionString")] TableClient shortUrlTable,
        FunctionContext executionContext)
    {
        var request = JsonSerializer.Deserialize<ShortenUrlRequest>(req.Body);
        
        var shortenId = GenerateRandomUrl();
        var shortUrl = await shortUrlTable.GetEntityIfExistsAsync<ShortUrlModel>(shortenId, shortenId);
        var i = 0;

        while (shortUrl.HasValue && i < 5)
        {
            shortenId = GenerateRandomUrl();
            shortUrl = shortUrlTable.GetEntity<ShortUrlModel>(shortenId, shortenId);
        }
        
        if (i >= 5)
        {
            var internalServerErrorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            internalServerErrorResponse.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await internalServerErrorResponse.WriteStringAsync("Failed to generate a unique ID for the shortened URL");
            return new ShortenUrlResponse
            {
                Response = internalServerErrorResponse,
                ShortUrl = null,
            };
        }
        
        var responseMessage = req.CreateResponse(HttpStatusCode.OK);
        responseMessage.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        await responseMessage.WriteStringAsync($"{this.SvrlesBackendHost}/{shortenId}");

        return new ShortenUrlResponse
        {
            Response = responseMessage,
            ShortUrl = new ShortUrlModel(request.Url, shortenId),
        };
    }

    private string GenerateRandomUrl()
    {
        var allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = new string(Enumerable.Range(0, 6).Select(_ => allowedChars[Random.Shared.Next(0, allowedChars.Length)]).ToArray());
        return result;
    }
}