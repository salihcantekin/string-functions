// Theme management functions
window.themeManager = {
    setTheme: function(themeClass, removeThemeClass) {
        const root = document.documentElement;
        if (removeThemeClass) {
            root.classList.remove(removeThemeClass);
        }
        if (themeClass) {
            root.classList.add(themeClass);
        }
    },
    
    getSystemPrefersDark: function() {
        // Return false if matchMedia is not supported (older browsers)
        if (!window.matchMedia) {
            return false;
        }
        return window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
};
