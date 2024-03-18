using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace BlazorApp.Shared.Models.ShortenedUrl;

public class ShortenUrlResponse
{
    public HttpResponseData Response { get; set; }
    
    [TableOutput("ShortUrlTable", Connection = "SvrlesDbConnectionString")]
    public ShortUrlModel ShortUrl { get; set; }
}