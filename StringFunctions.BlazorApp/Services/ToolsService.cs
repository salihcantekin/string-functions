using System.Net.Http.Json;
using StringFunctions.BlazorApp.Models;

namespace StringFunctions.BlazorApp.Services;

public class ToolsService(HttpClient httpClient)
{
    private ToolsData? toolsData;
    private bool isLoaded;

    public async Task<ToolsData> GetToolsDataAsync()
    {
        if (isLoaded && toolsData != null)
            return toolsData;

        try
        {
            toolsData = await httpClient.GetFromJsonAsync<ToolsData>("data/tools.json");
            isLoaded = true;

            return toolsData ?? new ToolsData();
        }
        catch
        {
            return new ToolsData();
        }
    }

    public async Task<List<ToolCategory>> GetCategoriesAsync()
    {
        var data = await GetToolsDataAsync();
        return [.. data.Categories.OrderBy(c => c.Order)];
    }

    public async Task<List<Tool>> GetAllToolsAsync()
    {
        var data = await GetToolsDataAsync();
        return [.. data.Categories.SelectMany(c => c.Tools)];
    }

    public async Task<Tool?> GetToolByIdAsync(string toolId)
    {
        var tools = await GetAllToolsAsync();
        return tools.FirstOrDefault(t => t.Id == toolId);
    }

    public async Task<Tool?> GetToolByRouteAsync(string route)
    {
        var tools = await GetAllToolsAsync();
        return tools.FirstOrDefault(t => t.Route == route || t.Route == "/" + route);
    }

    public async Task<List<Tool>> SearchToolsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        var tools = await GetAllToolsAsync();
        query = query.ToLower();

        return [.. tools.Where(t =>
            t.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            t.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            t.Keywords.Any(k => k.Contains(query, StringComparison.OrdinalIgnoreCase))
        )];
    }

    public async Task<ToolCategory?> GetCategoryByToolIdAsync(string toolId)
    {
        var data = await GetToolsDataAsync();

        return data.Categories.FirstOrDefault(c => c.Tools.Any(t => t.Id == toolId));
    }
}
