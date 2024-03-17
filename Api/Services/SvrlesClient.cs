using System.Text;
using System.Text.Json;

namespace Api.Services;

public class SvrlesClient(HttpClient httpClient)
{
    public async Task<string> ShortenUrl(string url)
    {
        var response = await httpClient.PostAsync($"api/shortUrl", new StringContent(JsonSerializer.Serialize(new { OriginUrl = url }), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }
}