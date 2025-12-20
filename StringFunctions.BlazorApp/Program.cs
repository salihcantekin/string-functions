using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using StringFunctions.BlazorApp;
using StringFunctions.BlazorApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddFluentUIComponents();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<UserPreferencesService>();
builder.Services.AddScoped<SeoService>();
builder.Services.AddScoped<ToolsService>();
builder.Services.AddScoped<FavoritesService>();
builder.Services.AddScoped<HttpClientPresetsService>();
builder.Services.AddScoped<SiteConfigService>();

await builder.Build().RunAsync();
