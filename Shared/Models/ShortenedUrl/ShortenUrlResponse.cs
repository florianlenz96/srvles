using Microsoft.Azure.Functions.Worker.Http;

namespace BlazorApp.Shared.Models.ShortenedUrl;

public class ShortenUrlResponse
{
    public HttpResponseData Response { get; set; }
}