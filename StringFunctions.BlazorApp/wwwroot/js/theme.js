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
        return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    }
};
