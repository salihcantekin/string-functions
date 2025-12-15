using System.Net.Http.Json;
using StringFunctions.BlazorApp.Models;

namespace StringFunctions.BlazorApp.Services;

public class HttpClientPresetsService(HttpClient httpClient)
{
    private HttpClientPresets? cachedPresets;

    public async Task<HttpClientPresets> GetPresetsAsync()
    {
        if (cachedPresets != null)
            return cachedPresets;

        try
        {
            cachedPresets = await httpClient.GetFromJsonAsync<HttpClientPresets>("data/http-presets.json");
            return cachedPresets ?? new HttpClientPresets();
        }
        catch
        {
            return new HttpClientPresets();
        }
    }

    public async Task<List<HeaderPreset>> GetCommonHeadersAsync()
    {
        var presets = await GetPresetsAsync();
        return presets.CommonHeaders;
    }

    public async Task<List<RequestPreset>> GetRequestPresetsAsync()
    {
        var presets = await GetPresetsAsync();
        return presets.RequestPresets;
    }

    public async Task<Dictionary<string, List<HeaderPreset>>> GetHeadersByCategoryAsync()
    {
        var headers = await GetCommonHeadersAsync();
        return headers
            .Where(h => h.IsEnabled)
            .GroupBy(h => h.Category)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
