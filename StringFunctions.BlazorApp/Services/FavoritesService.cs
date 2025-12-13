using Microsoft.JSInterop;
using System.Text.Json;

namespace StringFunctions.BlazorApp.Services;

public class FavoritesService(IJSRuntime jsRuntime)
{
    private const string StorageKey = "tool-favorites";
    private HashSet<string> favorites = [];
    private bool isLoaded;

    public event Action? OnFavoritesChanged;

    public async Task LoadAsync()
    {
        if (isLoaded) return;

        try
        {
            var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);

            if (!string.IsNullOrEmpty(json))
            {
                var favorites = JsonSerializer.Deserialize<List<string>>(json);
                if (favorites != null)
                {
                    this.favorites = [.. favorites];
                }
            }

            isLoaded = true;
        }
        catch
        {
            favorites = [];
            isLoaded = true;
        }
    }

    public async Task<bool> IsFavoriteAsync(string toolId)
    {
        await LoadAsync();
        return favorites.Contains(toolId);
    }

    public async Task ToggleFavoriteAsync(string toolId)
    {
        await LoadAsync();

        if (!favorites.Remove(toolId))
        {
            favorites.Add(toolId);
        }

        await SaveAsync();
        OnFavoritesChanged?.Invoke();
    }

    public async Task<List<string>> GetFavoritesAsync()
    {
        await LoadAsync();
        return [.. favorites];
    }

    public async Task<int> GetFavoritesCountAsync()
    {
        await LoadAsync();
        return favorites.Count;
    }

    private async Task SaveAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(favorites.ToList());
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch
        {
            // Silent fail
        }
    }

    public async Task ClearAllAsync()
    {
        favorites.Clear();
        await SaveAsync();
        OnFavoritesChanged?.Invoke();
    }
}
