window.seoHelper = {
    updateMetaTags: function (metadata) {
        // Update title
        document.title = metadata.title;
        
        // Update or create meta tags
        this.setMetaTag('name', 'title', metadata.title);
        this.setMetaTag('name', 'description', metadata.description);
        this.setMetaTag('name', 'keywords', metadata.keywords);
        
        // Open Graph
        this.setMetaTag('property', 'og:type', metadata.type);
        this.setMetaTag('property', 'og:url', metadata.url);
        this.setMetaTag('property', 'og:title', metadata.title);
        this.setMetaTag('property', 'og:description', metadata.description);
        this.setMetaTag('property', 'og:image', metadata.image);
        this.setMetaTag('property', 'og:site_name', metadata.siteName);
        
        // Twitter
        this.setMetaTag('property', 'twitter:card', 'summary_large_image');
        this.setMetaTag('property', 'twitter:url', metadata.url);
        this.setMetaTag('property', 'twitter:title', metadata.title);
        this.setMetaTag('property', 'twitter:description', metadata.description);
        this.setMetaTag('property', 'twitter:image', metadata.image);
        
        // Update canonical link
        this.setCanonicalLink(metadata.url);
    },
    
    setMetaTag: function (attribute, name, content) {
        let element = document.querySelector(`meta[${attribute}="${name}"]`);
        if (!element) {
            element = document.createElement('meta');
            element.setAttribute(attribute, name);
            document.head.appendChild(element);
        }
        element.setAttribute('content', content);
    },
    
    setCanonicalLink: function (url) {
        let link = document.querySelector('link[rel="canonical"]');
        if (!link) {
            link = document.createElement('link');
            link.setAttribute('rel', 'canonical');
            document.head.appendChild(link);
        }
        link.setAttribute('href', url);
    }
};
