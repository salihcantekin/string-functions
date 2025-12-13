using Microsoft.JSInterop;

namespace StringFunctions.BlazorApp.Services;

public enum AppTheme
{
    Light,
    Dark,
    System
}

public class ThemeService(IJSRuntime jsRuntime)
{
    private AppTheme _currentTheme = AppTheme.System;
    private bool _isDarkMode;

    public event Action? OnThemeChanged;

    public AppTheme CurrentTheme => _currentTheme;
    public bool IsDarkMode => _isDarkMode;

    public async Task InitializeAsync()
    {
        try
        {
            var savedTheme = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "app-theme");
            if (Enum.TryParse<AppTheme>(savedTheme, out var theme))
            {
                _currentTheme = theme;
            }
            
            await ApplyThemeAsync();
        }
        catch
        {
            // Ignore errors during initialization
        }
    }

    public async Task SetThemeAsync(AppTheme theme)
    {
        _currentTheme = theme;
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "app-theme", theme.ToString());
        await ApplyThemeAsync();
        OnThemeChanged?.Invoke();
    }

    private async Task ApplyThemeAsync()
    {
        _isDarkMode = _currentTheme switch
        {
            AppTheme.Light => false,
            AppTheme.Dark => true,
            AppTheme.System => await GetSystemPreferenceAsync(),
            _ => false
        };

        var themeClass = _isDarkMode ? "dark-theme" : "light-theme";
        await jsRuntime.InvokeVoidAsync("eval", $"document.documentElement.className = '{themeClass}'");
    }

    private async Task<bool> GetSystemPreferenceAsync()
    {
        try
        {
            return await jsRuntime.InvokeAsync<bool>("eval", 
                "window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches");
        }
        catch
        {
            return false;
        }
    }
}
