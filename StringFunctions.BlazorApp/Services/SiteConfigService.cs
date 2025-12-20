using System.Net.Http.Json;
using StringFunctions.BlazorApp.Models;

namespace StringFunctions.BlazorApp.Services;

public class SiteConfigService(HttpClient httpClient)
{
    private SiteConfig? siteConfig;
    private bool isLoaded;

    public async Task<SiteConfig> GetSiteConfigAsync()
    {
        if (isLoaded && siteConfig != null)
            return siteConfig;

        try
        {
            siteConfig = await httpClient.GetFromJsonAsync<SiteConfig>("data/site-config.json");
            isLoaded = true;

            return siteConfig ?? new SiteConfig();
        }
        catch
        {
            return new SiteConfig
            {
                SiteName = "String Functions",
                SiteUrl = "https://stringfunctions.net",
                SiteDescription = "Powerful tools collection for developers",
                Author = "String Functions",
                LogoPath = "images/logo.png",
                PaypalDonationUrl = "https://www.paypal.com/ncp/payment/6GVW3NYM36V22",
                DonationQRImagePath = "images/donation-qr.png"
            };
        }
    }
}
