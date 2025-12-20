using System.Net.Http.Json;
using StringFunctions.BlazorApp.Models;

namespace StringFunctions.BlazorApp.Services;

public class SiteConfigService(HttpClient httpClient)
{
    private SiteConfig? siteConfig;
    private bool isLoaded;

    // Default fallback values - keep in sync with site-config.json
    private static readonly SiteConfig DefaultConfig = new()
    {
        SiteName = "String Functions",
        SiteUrl = "https://stringfunctions.net",
        SiteDescription = "Powerful tools collection for developers",
        Author = "String Functions",
        LogoPath = "images/logo.png",
        PaypalDonationUrl = "https://www.paypal.com/ncp/payment/6GVW3NYM36V22",
        DonationQRImagePath = "images/donation-qr.png"
    };

    public async Task<SiteConfig> GetSiteConfigAsync()
    {
        if (isLoaded && siteConfig != null)
            return siteConfig;

        try
        {
            siteConfig = await httpClient.GetFromJsonAsync<SiteConfig>("data/site-config.json");
            isLoaded = true;

            return siteConfig ?? DefaultConfig;
        }
        catch
        {
            // If config file loading fails (network issue, file not found, etc.),
            // return default values to ensure the app still functions
            return DefaultConfig;
        }
    }
}
