namespace StringFunctions.BlazorApp.Models;

public class ToolsData
{
    public List<ToolCategory> Categories { get; set; } = new();
}

public class ToolCategory
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public int Order { get; set; }
    public List<Tool> Tools { get; set; } = new();
}

public class Tool
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OgTitle { get; set; } = string.Empty;
    public string OgDescription { get; set; } = string.Empty;
    public List<string> Keywords { get; set; } = new();
    public bool Featured { get; set; }
    public string AddedDate { get; set; } = string.Empty;
    
    public bool IsNew
    {
        get
        {
            if (string.IsNullOrEmpty(AddedDate)) return false;
            
            if (DateTime.TryParse(AddedDate, out var addedDate))
            {
                var daysSinceAdded = (DateTime.Now - addedDate).Days;
                return daysSinceAdded <= 7; // Show NEW badge for 7 days
            }
            
            return false;
        }
    }
}
