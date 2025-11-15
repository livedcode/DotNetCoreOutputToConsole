using DotNetCoreOutputToConsole;
using DotNetCoreOutputToConsole.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Set current environment
ConsoleOutputSettings.CurrentEnvironment = builder.Environment.EnvironmentName;

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/", async context =>
{
    await BrowserConsole.Log(context.Response, "Minimal API root loaded");
    await BrowserConsole.Info(context.Response, new { Route = "/", Time = DateTime.UtcNow });
    await BrowserConsole.Warn(context.Response, new { Warning = "This is a warning sample" });
    await BrowserConsole.Error(context.Response, new { Error = "Example error message" });
    await BrowserConsole.Trace(context.Response, "Trace message from minimal API");
    await BrowserConsole.Table(context.Response, new[]
    {
        new { Id = 1, Name = "Item A" },
        new { Id = 2, Name = "Item B" }
    });

    await context.Response.WriteAsync("<h1>Minimal API Console Demo</h1><p>Check browser console (F12).</p>");
});

app.MapGet("/api/error", async context =>
{
    await BrowserConsole.Error(context.Response, new { Message = "Error endpoint hit", Code = 500 });
    await context.Response.WriteAsync("<h1>Error endpoint</h1><p>Check browser console.</p>");
});

app.Run();

