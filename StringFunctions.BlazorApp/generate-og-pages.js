#!/usr/bin/env node

/**
 * Generates static HTML pages for each route with proper Open Graph metadata
 * for social media crawlers (Facebook, Twitter, LinkedIn, etc.)
 */

const fs = require('fs');
const path = require('path');

// Read tools.json to get all routes and metadata
const toolsJsonPath = path.join(__dirname, 'wwwroot', 'data', 'tools.json');
const toolsData = JSON.parse(fs.readFileSync(toolsJsonPath, 'utf8'));

// Base configuration
const baseUrl = 'https://stringfunctions.net';
const siteName = 'String Functions';
const defaultImage = `${baseUrl}/images/og-image.png`;

// Extract all tools from categories
const allTools = toolsData.categories.flatMap(category => 
  category.tools.map(tool => ({
    ...tool,
    category: category.name
  }))
);

// HTML template generator
function generateHtmlPage(metadata) {
  return `<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>${metadata.title}</title>
    
    <!-- Primary Meta Tags -->
    <meta name="title" content="${metadata.title}" />
    <meta name="description" content="${metadata.description}" />
    <meta name="keywords" content="${metadata.keywords}" />
    <meta name="robots" content="index, follow" />
    <meta name="language" content="English" />
    <meta name="author" content="${siteName}" />
    <link rel="canonical" href="${metadata.url}" />
    
    <!-- Open Graph / Facebook -->
    <meta property="og:type" content="website" />
    <meta property="og:url" content="${metadata.url}" />
    <meta property="og:title" content="${metadata.title}" />
    <meta property="og:description" content="${metadata.description}" />
    <meta property="og:image" content="${metadata.image}" />
    <meta property="og:image:secure_url" content="${metadata.image}" />
    <meta property="og:image:width" content="1200" />
    <meta property="og:image:height" content="630" />
    <meta property="og:image:type" content="image/png" />
    <meta property="og:image:alt" content="${metadata.title}" />
    <meta property="og:site_name" content="${siteName}" />
    <meta property="og:locale" content="en_US" />
    
    <!-- Twitter -->
    <meta property="twitter:card" content="summary_large_image" />
    <meta property="twitter:url" content="${metadata.url}" />
    <meta property="twitter:title" content="${metadata.title}" />
    <meta property="twitter:description" content="${metadata.description}" />
    <meta property="twitter:image" content="${metadata.image}" />
    <meta property="twitter:image:alt" content="${metadata.title}" />
    
    <!-- Load Blazor app (crawlers won't execute this) -->
    <base href="/" />
    <link rel="preload" href="/_framework/blazor.webassembly.js" as="script" />
    <link rel="preload" href="/css/app.css" as="style" />
    <link href="/_content/Microsoft.FluentUI.AspNetCore.Components/css/reboot.css" rel="stylesheet" />
    <link href="/css/app.css" rel="stylesheet" />
    <link rel="icon" type="image/x-icon" href="/favicon.ico" />
    
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh;
            margin: 0;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }
        .loading {
            text-align: center;
        }
        .spinner {
            border: 4px solid rgba(255, 255, 255, 0.3);
            border-top: 4px solid white;
            border-radius: 50%;
            width: 40px;
            height: 40px;
            animation: spin 1s linear infinite;
            margin: 0 auto 20px;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
</head>
<body>
    <div id="app">
        <div class="loading">
            <div class="spinner"></div>
            <p>Loading ${metadata.title}...</p>
        </div>
    </div>
    
    <div id="blazor-error-ui" style="display:none;background:#ffdddd;color:#a00;padding:10px 20px;position:fixed;bottom:0;left:0;right:0;z-index:9999;">
        An unexpected error occurred.
        <a href="javascript:location.reload()" class="reload">Reload</a>
        <a class="dismiss" onclick="this.parentElement.style.display='none'" style="cursor:pointer;float:right;">X</a>
    </div>
    
    <!-- Load Blazor WebAssembly -->
    <script src="/_framework/blazor.webassembly.js"></script>
</body>
</html>`;
}

// Generate pages for all tools
const outputDir = path.join(__dirname, 'wwwroot');

console.log('Generating Open Graph pages...');
console.log(`Found ${allTools.length} tools`);

let generatedCount = 0;

allTools.forEach(tool => {
  const route = tool.route.replace(/^\//, ''); // Remove leading slash
  const metadata = {
    title: tool.ogTitle || `${tool.name} - ${siteName}`,
    description: tool.ogDescription || tool.description,
    keywords: tool.keywords.join(', '),
    url: `${baseUrl}${tool.route}`,
    image: defaultImage
  };
  
  const html = generateHtmlPage(metadata);
  
  // Create directory for the route if needed
  const routeDir = path.join(outputDir, route);
  if (!fs.existsSync(routeDir)) {
    fs.mkdirSync(routeDir, { recursive: true });
  }
  
  // Write index.html in the route directory
  const filePath = path.join(routeDir, 'index.html');
  fs.writeFileSync(filePath, html);
  
  console.log(`✓ Generated: ${route}/index.html`);
  generatedCount++;
});

console.log(`\nSuccessfully generated ${generatedCount} Open Graph pages!`);
console.log('\nThese pages will be served to social media crawlers while regular');
console.log('users will be redirected to the main Blazor application.');
