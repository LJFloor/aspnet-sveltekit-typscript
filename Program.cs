using Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/ping", () => "Pong!");

app.UseSvelteKit(builder.Environment.WebRootPath);

app.Run();
