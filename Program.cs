using Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/ping", async (context) => {
    await context.Response.WriteAsJsonAsync(context.Request.Query);
});

app.UseSvelteKit(builder.Environment.WebRootPath);

app.Run();
