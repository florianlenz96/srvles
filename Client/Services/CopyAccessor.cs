using Microsoft.JSInterop;

namespace BlazorApp.Client.Services;

public class CopyAccessor
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime;
    
    public CopyAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CopyToClipboard.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }

    public async Task CopyShortUrl(string url)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("copyShortUrl", url);
    }
}