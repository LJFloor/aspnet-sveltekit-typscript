namespace Middleware;

public class SvelteKitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _webRootPath;

    public SvelteKitMiddleware(RequestDelegate next, string webRootPath)
    {
        _next = next;
        _webRootPath = webRootPath;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api")) {
            await _next(context);
            return;
        }

        context.Response.ContentType = "text/html";

        // When using prerendering
        var requestPath = context.Request.Path.Value.TrimStart('/').Split('?')[0];
        var filePath = Path.Combine(_webRootPath, $"{(string.IsNullOrEmpty(requestPath) ? "index" : requestPath)}.html");
        if (File.Exists(filePath))
        {
            await context.Response.SendFileAsync(filePath);
            return;
        }

        // When SPA
        await _next(context);
        if (context.Response.StatusCode == 404)
        {
            context.Response.StatusCode = 200;
            await context.Response.SendFileAsync(Path.Combine(_webRootPath, "200.html"));
        }
    }
}

public static class SvelteKitMiddlewareExtensions
{
    /// <summary>
    /// Pass requests that don't start with /api to SvelteKit
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseSvelteKit(this WebApplication app)
    {
        app.UseStaticFiles();
        return app.UseMiddleware<SvelteKitMiddleware>(Path.Combine(app.Environment.ContentRootPath, "wwwroot"));
    }
}
