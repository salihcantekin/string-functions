using StringFunctions.BlazorApp.Models;
using Microsoft.JSInterop;

namespace StringFunctions.BlazorApp.Services;

public class SeoService(IJSRuntime jsRuntime, ToolsService toolsService)
{
    private readonly string baseUrl = "https://stringfunctions.net";
    private readonly string siteName = "String Functions";
    private readonly string defaultImage = "/images/logo-large.png";

    public async Task<PageMetadata> GetPageMetadataAsync(string route)
    {
        // Clean the route
        var cleanRoute = route.TrimStart('/').ToLowerInvariant();
        
        // Try to get tool-specific metadata from tools.json
        var tool = await toolsService.GetToolByRouteAsync("/" + cleanRoute);
        if (tool != null && !string.IsNullOrEmpty(tool.OgTitle))
        {
            return new PageMetadata
            {
                Title = tool.OgTitle,
                Description = tool.OgDescription,
                Keywords = string.Join(", ", tool.Keywords),
                Url = $"{baseUrl}{tool.Route}",
                Tags = tool.Keywords,
                BreadcrumbItems = await GetBreadcrumbForToolAsync(tool)
            };
        }
        
        // Fall back to hardcoded metadata for routes not in tools.json
        return GetDefaultPageMetadata(cleanRoute);
    }

    private async Task<List<string>> GetBreadcrumbForToolAsync(Tool tool)
    {
        var category = await toolsService.GetCategoryByToolIdAsync(tool.Id);
        if (category != null)
        {
            return ["Home", category.Name, tool.Name];
        }
        return ["Home", tool.Name];
    }

    private PageMetadata GetDefaultPageMetadata(string route)
    {
        var metadata = route switch
        {
            "" or "index" => new PageMetadata
            {
                Title = "String Functions - Developer Tools for Text & JSON",
                Description = "Free online string manipulation tools for developers. Base64, JSON formatter, regex tools, case converter, URL encoding, hash generator and more.",
                Keywords = "string tools, text tools, developer tools, base64, json formatter, regex, url encode, hash generator, online tools",
                Url = baseUrl,
                Tags = ["developer tools", "string manipulation", "text processing", "online tools"]
            },
            
            "base64" => new PageMetadata
            {
                Title = "Base64 Encode/Decode - Free Base64 Converter Tool",
                Description = "Fast and secure Base64 encoder and decoder. Convert text to Base64 or decode Base64 to text instantly. Supports UTF-8, ASCII, Unicode encoding.",
                Keywords = "base64 encode, base64 decode, base64 converter, base64 online, base64 tool, encode decode",
                Url = $"{baseUrl}/base64",
                Tags = ["base64", "encoding", "decoding", "converter"],
                BreadcrumbItems = ["Home", "Encode/Decode", "Base64"]
            },
            
            "case-converter" => new PageMetadata
            {
                Title = "Case Converter - camelCase, snake_case, PascalCase",
                Description = "Convert text to camelCase, PascalCase, snake_case, kebab-case, UPPER CASE and more. Perfect for developers and programmers. Fast and free.",
                Keywords = "case converter, camelCase, snake_case, PascalCase, kebab-case, text converter, change case",
                Url = $"{baseUrl}/case-converter",
                Tags = ["case converter", "text formatting", "naming conventions"],
                BreadcrumbItems = ["Home", "Text Operations", "Case Converter"]
            },
            
            "json-formatter" => new PageMetadata
            {
                Title = "JSON Formatter & Validator - Format JSON Online",
                Description = "Format, validate, minify, and beautify JSON data online. Syntax highlighting and error detection for developers. Free JSON formatter.",
                Keywords = "json formatter, json validator, json beautifier, json minify, format json, validate json",
                Url = $"{baseUrl}/json-formatter",
                Tags = ["json", "formatter", "validator", "beautifier"],
                BreadcrumbItems = ["Home", "JSON Tools", "JSON Formatter"]
            },
            
            "json-to-csharp" => new PageMetadata
            {
                Title = "JSON to C# Class Generator - Convert JSON to C#",
                Description = "Generate C# classes from JSON instantly. Free online tool to convert JSON to C# POCO classes with proper naming conventions and data types.",
                Keywords = "json to c#, json to csharp, generate c# class, json converter, c# class generator",
                Url = $"{baseUrl}/json-to-csharp",
                Tags = ["json", "c#", "code generator", "converter"],
                BreadcrumbItems = ["Home", "JSON Tools", "JSON to C#"]
            },
            
            "json-to-java" => new PageMetadata
            {
                Title = "JSON to Java POJO Generator - Convert JSON to Java",
                Description = "Generate Java POJO classes from JSON data. Free tool to convert JSON to Java objects with Gson and Jackson annotations support.",
                Keywords = "json to java, json to pojo, java class generator, json converter, pojo generator",
                Url = $"{baseUrl}/json-to-java",
                Tags = ["json", "java", "pojo", "code generator"],
                BreadcrumbItems = ["Home", "JSON Tools", "JSON to Java"]
            },
            
            "json-to-typescript" => new PageMetadata
            {
                Title = "JSON to TypeScript - Generate TS Interfaces Online",
                Description = "Generate TypeScript interfaces from JSON. Free online tool to convert JSON to TypeScript types and interfaces instantly.",
                Keywords = "json to typescript, typescript interface generator, json to ts, typescript types",
                Url = $"{baseUrl}/json-to-typescript",
                Tags = ["json", "typescript", "interface", "code generator"],
                BreadcrumbItems = ["Home", "JSON Tools", "JSON to TypeScript"]
            },
            
            "json-compare" => new PageMetadata
            {
                Title = "JSON Compare - Compare & Diff JSON Files Online",
                Description = "Compare two JSON documents online. Find differences, visualize changes, and validate JSON structure. Free JSON diff tool for developers.",
                Keywords = "json compare, json diff, compare json, json comparison, json difference",
                Url = $"{baseUrl}/json-compare",
                Tags = ["json", "compare", "diff", "comparison"],
                BreadcrumbItems = ["Home", "JSON Tools", "JSON Compare"]
            },
            
            "url-encode" => new PageMetadata
            {
                Title = "URL Encoder/Decoder - Free URL Encode Decode Tool",
                Description = "Encode and decode URLs online. Convert special characters for web URLs. Free URL encoder and decoder tool for developers.",
                Keywords = "url encode, url decode, url encoder, urlencode, percent encoding, url converter",
                Url = $"{baseUrl}/url-encode",
                Tags = ["url", "encoding", "decoding", "web"],
                BreadcrumbItems = ["Home", "Encode/Decode", "URL Encode"]
            },
            
            "hash-generator" => new PageMetadata
            {
                Title = "Hash Generator - MD5, SHA1, SHA256, SHA512 Online",
                Description = "Generate cryptographic hashes online. Create MD5, SHA1, SHA256, SHA512 hashes from text. Free hash generator tool for developers.",
                Keywords = "hash generator, md5 generator, sha256 generator, sha1, sha512, cryptographic hash",
                Url = $"{baseUrl}/hash-generator",
                Tags = ["hash", "md5", "sha256", "cryptography"],
                BreadcrumbItems = ["Home", "Encode/Decode", "Hash Generator"]
            },
            
            "regex-search" => new PageMetadata
            {
                Title = "Regex Tester - Test Regular Expressions Online",
                Description = "Test and debug regular expressions online. Live regex tester with syntax highlighting and match details. Free regex tool for developers.",
                Keywords = "regex tester, regular expression tester, regex online, regex test, pattern matching",
                Url = $"{baseUrl}/regex-search",
                Tags = ["regex", "pattern matching", "search", "testing"],
                BreadcrumbItems = ["Home", "Regex Tools", "Regex Search"]
            },
            
            "regex-replace" => new PageMetadata
            {
                Title = "Regex Replace - Find & Replace with RegEx Online",
                Description = "Replace text using regular expressions online. Pattern-based find and replace with regex support. Free tool for developers.",
                Keywords = "regex replace, regex find replace, regular expression replace, pattern replace",
                Url = $"{baseUrl}/regex-replace",
                Tags = ["regex", "replace", "find", "pattern matching"],
                BreadcrumbItems = ["Home", "Regex Tools", "Regex Replace"]
            },
            
            "extract" => new PageMetadata
            {
                Title = "Text Extract Tool - Extract Patterns with Regex",
                Description = "Extract text patterns using regular expressions. Extract emails, URLs, numbers, and custom patterns from text. Free extraction tool.",
                Keywords = "text extract, extract patterns, regex extract, pattern extraction, text parsing",
                Url = $"{baseUrl}/extract",
                Tags = ["extract", "regex", "pattern matching", "parsing"],
                BreadcrumbItems = ["Home", "Regex Tools", "Extract"]
            },
            
            "reverse" => new PageMetadata
            {
                Title = "String Reverse Tool - Reverse Text Online Free",
                Description = "Reverse text strings and lines online. Free tool to reverse character order, words, or entire text. Simple and fast string reverser.",
                Keywords = "reverse string, reverse text, text reverser, string reversal, reverse words",
                Url = $"{baseUrl}/reverse",
                Tags = ["reverse", "string manipulation", "text tools"],
                BreadcrumbItems = ["Home", "Text Operations", "String Reverse"]
            },
            
            "trim-pad" => new PageMetadata
            {
                Title = "Trim & Pad Tool - Remove Whitespace & Add Padding",
                Description = "Trim whitespace or add padding to text. Remove leading/trailing spaces, add left/right padding. Free text formatting tool.",
                Keywords = "trim text, pad text, remove whitespace, text padding, trim whitespace",
                Url = $"{baseUrl}/trim-pad",
                Tags = ["trim", "pad", "whitespace", "formatting"],
                BreadcrumbItems = ["Home", "Text Operations", "Trim/Pad"]
            },
            
            "line-counter" => new PageMetadata
            {
                Title = "Line Counter - Count Lines, Words & Characters",
                Description = "Count lines, words, and characters in text. Free online counter tool for developers and writers. Real-time counting.",
                Keywords = "line counter, word counter, character counter, count lines, text statistics",
                Url = $"{baseUrl}/line-counter",
                Tags = ["counter", "statistics", "text analysis"],
                BreadcrumbItems = ["Home", "Text Operations", "Line Counter"]
            },
            
            "line-operations" => new PageMetadata
            {
                Title = "Line Operations - Sort, Filter & Dedupe (Notepad++)",
                Description = "Advanced line operations tool. Sort lines, remove duplicates, filter by pattern, number lines. Notepad++ style text processing.",
                Keywords = "sort lines, remove duplicates, filter lines, line operations, text processing",
                Url = $"{baseUrl}/line-operations",
                Tags = ["lines", "sort", "filter", "dedupe", "text processing"],
                BreadcrumbItems = ["Home", "Text Operations", "Line Operations"]
            },
            
            "compare" => new PageMetadata
            {
                Title = "Text Compare Tool - Compare Two Texts Side by Side",
                Description = "Compare two text documents side by side. Find differences and similarities. Free online text comparison tool for developers.",
                Keywords = "text compare, compare text, text diff, text comparison, compare documents",
                Url = $"{baseUrl}/compare",
                Tags = ["compare", "diff", "text analysis"],
                BreadcrumbItems = ["Home", "Text Operations", "Compare"]
            },
            
            "datetime-formatter" => new PageMetadata
            {
                Title = "DateTime Formatter - Test C# DateTime Formats",
                Description = "Test C# DateTime format strings online. Preview different date and time formats instantly. Free tool for .NET developers.",
                Keywords = "datetime formatter, c# datetime, date format, time format, datetime format string",
                Url = $"{baseUrl}/datetime-formatter",
                Tags = ["datetime", "formatting", "c#", "dotnet"],
                BreadcrumbItems = ["Home", "Developer Tools", "DateTime Formatter"]
            },
            
            "string-formatter" => new PageMetadata
            {
                Title = "String Formatter - Test C# String Format Online",
                Description = "Test C# string formatting and interpolation online. Preview composite formatting, string interpolation, and format specifiers.",
                Keywords = "string formatter, c# string format, string interpolation, format string, c# formatting",
                Url = $"{baseUrl}/string-formatter",
                Tags = ["string", "formatting", "c#", "dotnet"],
                BreadcrumbItems = ["Home", "Developer Tools", "String Formatter"]
            },
            
            "guid-generator" => new PageMetadata
            {
                Title = "GUID Generator - Generate UUID/GUID Online",
                Description = "Generate GUIDs (UUIDs) online. Create random GUIDs in various formats. Free UUID/GUID generator for developers.",
                Keywords = "guid generator, uuid generator, generate guid, create uuid, unique identifier",
                Url = $"{baseUrl}/guid-generator",
                Tags = ["guid", "uuid", "generator", "unique id"],
                BreadcrumbItems = ["Home", "Developer Tools", "GUID Generator"]
            },
            
            "password-generator" => new PageMetadata
            {
                Title = "Password Generator - Generate Strong Passwords",
                Description = "Generate strong, secure passwords online. Customize length and character types. Free random password generator tool.",
                Keywords = "password generator, random password, strong password, password creator, secure password",
                Url = $"{baseUrl}/password-generator",
                Tags = ["password", "generator", "security", "random"],
                BreadcrumbItems = ["Home", "Developer Tools", "Password Generator"]
            },
            
            "http-client" => new PageMetadata
            {
                Title = "HTTP Client - Test HTTP Requests & REST APIs",
                Description = "Build and send HTTP requests online. Test REST APIs with custom headers, body, and methods. Free HTTP client for developers.",
                Keywords = "http client, rest client, api tester, http request, test api, rest api tool",
                Url = $"{baseUrl}/http-client",
                Tags = ["http", "rest", "api", "testing", "web"],
                BreadcrumbItems = ["Home", "Developer Tools", "HTTP Client"]
            },
            
            "number-converter" => new PageMetadata
            {
                Title = "Number Converter - Binary, Hex, Decimal, Octal",
                Description = "Convert numbers between binary, decimal, hexadecimal, and octal. Free online number system converter for programmers and students.",
                Keywords = "number converter, binary converter, hex converter, decimal to binary, hex to decimal, octal converter",
                Url = $"{baseUrl}/number-converter",
                Tags = ["number", "converter", "binary", "hex", "octal"],
                BreadcrumbItems = ["Home", "Developer Tools", "Number Converter"]
            },
            
            _ => new PageMetadata
            {
                Title = $"{route.Replace("-", " ")} - String Functions",
                Description = "Developer tools for string manipulation, JSON processing, and text operations.",
                Keywords = "developer tools, string tools, online tools",
                Url = $"{baseUrl}/{route}",
                Tags = ["tools", "developer"]
            }
        };

        // Set common properties
        metadata.ImageUrl ??= defaultImage;
        
        return metadata;
    }

    public async Task UpdateMetadataAsync(PageMetadata metadata)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("seoHelper.updateMetaTags", new
            {
                title = metadata.Title,
                description = metadata.Description,
                url = metadata.Url,
                keywords = metadata.Keywords,
                image = metadata.ImageUrl,
                type = metadata.Type,
                siteName = siteName
            });
        }
        catch
        {
            // JS interop might not be ready, ignore
        }
    }

    public string GenerateStructuredData(PageMetadata metadata)
    {
        var breadcrumbList = metadata.BreadcrumbItems.Count > 0 
            ? GenerateBreadcrumbSchema(metadata.BreadcrumbItems, metadata.Url)
            : "";

        var webPageSchema = $$"""
        {
            "@context": "https://schema.org",
            "@type": "{{metadata.SchemaType}}",
            "name": "{{metadata.Title}}",
            "description": "{{metadata.Description}}",
            "url": "{{metadata.Url}}",
            "applicationCategory": "DeveloperApplication",
            "operatingSystem": "Any",
            "offers": {
                "@type": "Offer",
                "price": "0",
                "priceCurrency": "USD"
            },
            "creator": {
                "@type": "Organization",
                "name": "{{siteName}}"
            }
        }
        """;

        return breadcrumbList.Length > 0 
            ? $"[{webPageSchema},{breadcrumbList}]"
            : $"[{webPageSchema}]";
    }

    private string GenerateBreadcrumbSchema(List<string> items, string url)
    {
        if (items.Count == 0) return "";

        var listItems = new List<string>();
        var currentPath = baseUrl;
        
        for (int i = 0; i < items.Count; i++)
        {
            if (i > 0)
            {
                currentPath += "/" + items[i].ToLowerInvariant().Replace(" ", "-");
            }
            
            listItems.Add($$"""
                {
                    "@type": "ListItem",
                    "position": {{i + 1}},
                    "name": "{{items[i]}}",
                    "item": "{{(i == items.Count - 1 ? url : currentPath)}}"
                }
            """);
        }

        return $$"""
        {
            "@context": "https://schema.org",
            "@type": "BreadcrumbList",
            "itemListElement": [{{string.Join(",", listItems)}}]
        }
        """;
    }
}
