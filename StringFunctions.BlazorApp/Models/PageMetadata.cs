namespace StringFunctions.BlazorApp.Models;

public class PageMetadata
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Type { get; set; } = "website";
    public string? Author { get; set; }
    public DateTime? PublishedTime { get; set; }
    public DateTime? ModifiedTime { get; set; }
    public List<string> Tags { get; set; } = [];
    
    // Schema.org structured data
    public string SchemaType { get; set; } = "WebApplication";
    public List<string> BreadcrumbItems { get; set; } = [];
}
