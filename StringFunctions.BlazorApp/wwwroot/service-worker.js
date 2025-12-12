// String Functions - Service Worker for caching
// Increment version when deploying new builds to force cache refresh
const CACHE_VERSION = 'v2';
const CACHE_NAME = `string-functions-${CACHE_VERSION}`;

// Files to cache immediately (excluding framework files which change frequently)
const PRECACHE_URLS = [
    '/',
    '/index.html',
    '/css/app.css',
    '/manifest.json'
];

// Install event - cache critical resources
self.addEventListener('install', event => {
    console.log('[SW] Installing new version:', CACHE_VERSION);
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => cache.addAll(PRECACHE_URLS))
            .then(() => self.skipWaiting())
    );
});

// Activate event - clean up old caches
self.addEventListener('activate', event => {
    console.log('[SW] Activating new version:', CACHE_VERSION);
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames
                    .filter(cacheName => cacheName.startsWith('string-functions-') && cacheName !== CACHE_NAME)
                    .map(cacheName => {
                        console.log('[SW] Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    })
            );
        }).then(() => self.clients.claim())
    );
});

// Fetch event - network-first for framework, cache-first for static assets
self.addEventListener('fetch', event => {
    // Skip non-GET requests
    if (event.request.method !== 'GET') {
        return;
    }

    const url = new URL(event.request.url);

    // For framework files (_framework/*), ALWAYS use network-first to avoid WASM version mismatch
    if (url.pathname.startsWith('/_framework/')) {
        event.respondWith(
            fetch(event.request)
                .then(response => {
                    // Cache the new framework files
                    if (response.status === 200) {
                        const responseClone = response.clone();
                        caches.open(CACHE_NAME).then(cache => {
                            cache.put(event.request, responseClone);
                        });
                    }
                    return response;
                })
                .catch(() => {
                    // Only fallback to cache if network fails
                    return caches.match(event.request);
                })
        );
        return;
    }

    // For static assets, use cache-first with network fallback
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                if (response) {
                    return response;
                }
                return fetch(event.request).then(response => {
                    if (response.status === 200) {
                        const responseClone = response.clone();
                        caches.open(CACHE_NAME).then(cache => {
                            cache.put(event.request, responseClone);
                        });
                    }
                    return response;
                });
            })
    );
});
