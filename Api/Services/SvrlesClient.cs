namespace Api.Services;

public class SvrlesClient(HttpClient httpClient)
{
    public async Task<string> ShortenUrl(string url)
    {
        /*var response = await httpClient.PostAsync($"shorten/{url}", null);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();*/
        
        // Generate a random string to simulate the shortened URL
        // the value should be a random string of 6 characters
        var result = new string(Enumerable.Range(0, 6).Select(_ => (char)('a' + Random.Shared.Next(0, 26))).ToArray());
        return await Task.FromResult("svrl.es/" + result);
    }
}