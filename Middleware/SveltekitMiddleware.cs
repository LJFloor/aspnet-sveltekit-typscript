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
        await _next(context);
        if (!context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 404)
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
        return app.UseMiddleware<SvelteKitMiddleware>(app.Environment.WebRootPath);
    }
}
