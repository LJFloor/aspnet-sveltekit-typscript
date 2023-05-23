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
Build the ASP.NET app in Release mode. Then start the ASP.NET app.

ASP.NET is configured to serve the SvelteKit app to requests that do NOT start with `/api`.

## How does it work?

You can use the following code in your SvelteKit app:
```js
fetch('/api/users/1');
```

During development SvelteKit will proxy this to the backend, and during production ASP.NET will handle this by itself.

Websockets are supported as well:
```js
const ws = new WebSocket('ws://' + window.location.host + '/api/websockets/notifications');
```
