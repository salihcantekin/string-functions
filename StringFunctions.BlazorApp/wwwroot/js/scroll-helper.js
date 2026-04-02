// Scroll Helper - Smooth scroll to element
export function scrollToElement(element) {
    if (!element) return;
    
    try {
        element.scrollIntoView({ 
            behavior: 'smooth', 
            block: 'start',
            inline: 'nearest'
        });
    } catch (error) {
        // Fallback for older browsers
        element.scrollIntoView(true);
    }
}

// Scroll to top of page
export function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Scroll to bottom of page
export function scrollToBottom() {
    window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
}
