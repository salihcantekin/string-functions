using Microsoft.JSInterop;

namespace StringFunctions.BlazorApp.Services;

public class UserPreferencesService
{
    private readonly IJSRuntime _jsRuntime;
    private const string LastVisitedPageKey = "last-visited-page";

    public UserPreferencesService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string?> GetLastVisitedPageAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", LastVisitedPageKey);
        }
        catch
        {
            return null;
        }
    }

    public async Task SetLastVisitedPageAsync(string path)
    {
        try
        {
            // Don't save home page as last visited
            if (string.IsNullOrEmpty(path) || path == "/")
                return;

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", LastVisitedPageKey, path);
        }
        catch
        {
            // Ignore storage errors
        }
    }
}
