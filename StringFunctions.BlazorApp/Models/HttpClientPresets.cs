namespace StringFunctions.BlazorApp.Models;

public class HttpClientPresets
{
    public List<HeaderPreset> CommonHeaders { get; set; } = new();
    public List<RequestPreset> RequestPresets { get; set; } = new();
}

public class HeaderPreset
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
}

public class RequestPreset
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public string Url { get; set; } = string.Empty;
    public List<HeaderPreset> Headers { get; set; } = new();
    public string? Body { get; set; }
    public string? BodyType { get; set; }
}
