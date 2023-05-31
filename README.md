## Installation
Install the required node modules
```shell
npm install
```

## Development
Run both the aspnet app and the SvelteKit devserver:
```shell
npm run dev   // http://localhost:5173
```
```shell
dotnet run    // http://localhost:5153
```

Then access your app from the SvelteKit url, http://localhost:5173.

In SvelteKit, all requests that start with `/api` will be proxied to the aspnet app.

Example:
```
http://localhost:5173/api/users/1 => http://localhost:5153/api/users/1
```

## Deployment
Run the ASP.NET app in Release mode. 
```shell
dotnet run --configuration Release
```

This will automatically update the node modules and build SvelteKit.

ASP.NET is configured to serve the SvelteKit app to requests that do NOT start with `/api`.

# Prerendering
Prerendering has a noticable performance advantage. Browsers tend to flicker less on the initial page
request. You can enable prerendering in `.src/routes/+layout.js`. 

One downside is that if you enable prerendering, you will not be able to use slugs on the fronted 
(e.g. `http://localhost:5173/admin/users/1`)

## How does it work?

You can use the following code in your SvelteKit app:
```js
fetch('/api/users/1');
```

During development SvelteKit will proxy this to the backend, and during production ASP.NET will handle this by itself.

Websockets are also supported, although they require a bit more configuration since they don't support relative paths:
```js
const ws = new WebSocket((location.protocol === "https:" ? "wss://" : "ws://") + location.host + "/api/ws/status");
```
