using System.Text.Json;
using Microsoft.JSInterop;

namespace BlazorApp.Client.Services;

public class CookieStorageAccessor
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime;
    
    public CookieStorageAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CookieStorageAccessor.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }

    public async Task<T?> GetValueAsync<T>(string key)
    {
        await WaitForReference();
        var json = await _accessorJsRef.Value.InvokeAsync<string?>("get", key);
        if (json is null)
        {
            return default!;
        }

        var result = JsonSerializer.Deserialize<T>(json);

        return result;
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await WaitForReference();
        var json = JsonSerializer.Serialize(value);
        await _accessorJsRef.Value.InvokeVoidAsync("set", key, json);
    }
}