namespace StringFunctions.BlazorApp.Models;

public class SiteConfig
{
    public string SiteName { get; set; } = string.Empty;
    public string SiteUrl { get; set; } = string.Empty;
    public string SiteDescription { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string LogoPath { get; set; } = string.Empty;
    public string PaypalDonationUrl { get; set; } = string.Empty;
    public string DonationQRImagePath { get; set; } = string.Empty;
    public SeoConfig Seo { get; set; } = new();
}

public class SeoConfig
{
    public string DefaultImageUrl { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string TwitterCardType { get; set; } = string.Empty;
}
