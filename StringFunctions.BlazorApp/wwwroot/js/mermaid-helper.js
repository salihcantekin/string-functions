// Mermaid.js Diagram Rendering
let currentTheme = 'default';

window.renderMermaid = async function (code, config) {
    try {
        const configObj = JSON.parse(config);

        // Re-initialize if theme changed
        if (configObj.theme !== currentTheme) {
            mermaid.initialize(configObj);
            currentTheme = configObj.theme;
        }

        // Generate unique ID
        const id = 'mermaid-' + Date.now();

        // Render the diagram
        const { svg } = await mermaid.render(id, code);

        return svg;
    } catch (error) {
        console.error('Mermaid rendering error:', error);
        throw new Error('Syntax error in diagram code: ' + error.message);
    }
};

window.downloadMermaidSvg = function (filename) {
    try {
        const svgElement = document.querySelector('#mermaid-diagram svg');
        if (!svgElement) {
            throw new Error('No diagram to export');
        }

        const svgData = new XMLSerializer().serializeToString(svgElement);
        const blob = new Blob([svgData], { type: 'image/svg+xml' });
        const url = URL.createObjectURL(blob);

        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();

        URL.revokeObjectURL(url);
    } catch (error) {
        console.error('Export error:', error);
        throw error;
    }
};

window.downloadMermaidPng = function (filename, scale = 2) {
    try {
        const svgElement = document.querySelector('#mermaid-diagram svg');
        if (!svgElement) {
            throw new Error('No diagram to export');
        }

        // Get SVG dimensions
        const bbox = svgElement.getBBox();
        const width = bbox.width || svgElement.width.baseVal.value || 800;
        const height = bbox.height || svgElement.height.baseVal.value || 600;

        // Create canvas
        const canvas = document.createElement('canvas');
        canvas.width = width * scale;
        canvas.height = height * scale;
        const ctx = canvas.getContext('2d');

        // Scale for better quality
        ctx.scale(scale, scale);

        // White background
        ctx.fillStyle = 'white';
        ctx.fillRect(0, 0, width, height);

        // Convert SVG to image
        const svgData = new XMLSerializer().serializeToString(svgElement);
        const img = new Image();
        const svgBlob = new Blob([svgData], { type: 'image/svg+xml;charset=utf-8' });
        const url = URL.createObjectURL(svgBlob);

        img.onload = function() {
            ctx.drawImage(img, 0, 0, width, height);
            URL.revokeObjectURL(url);

            // Download PNG
            canvas.toBlob(function(blob) {
                const pngUrl = URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = pngUrl;
                link.download = filename;
                link.click();
                URL.revokeObjectURL(pngUrl);
            }, 'image/png');
        };

        img.src = url;
    } catch (error) {
        console.error('PNG export error:', error);
        throw error;
    }
};

// Generic download text file function
window.downloadTextFile = function (content, filename) {
    try {
        const blob = new Blob([content], { type: 'text/plain;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();
        URL.revokeObjectURL(url);
    } catch (error) {
        console.error('Download error:', error);
        throw error;
    }
};

// Re-initialize Mermaid when theme changes
window.reinitializeMermaid = function (config) {
    const configObj = JSON.parse(config);
    mermaid.initialize(configObj);
    mermaidInitialized = true;
};
