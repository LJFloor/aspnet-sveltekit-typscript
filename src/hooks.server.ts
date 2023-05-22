import type { Handle } from '@sveltejs/kit';

export const handle = (async ({ event, resolve }) => {

    // If a url starts with /api, foward the request to the backend
    if (event.url.pathname.startsWith('/api')) {
        const newUrl = event.request.url.replace(/http:\/\/localhost:[0-9]+/, "http://localhost:5153");
        return await event.fetch(newUrl, event.request);
    }

    // Else just run the request
    return await resolve(event);
}) satisfies Handle;``
