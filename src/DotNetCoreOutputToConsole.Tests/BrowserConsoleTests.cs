using DotNetCoreOutputToConsole.Config;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreOutputToConsole.Tests;

public class BrowserConsoleTests
{
    [Fact]
    public async Task WritesConsoleLogInAllowedEnvironment()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "Development" };
        ConsoleOutputSettings.CurrentEnvironment = "Development";

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await BrowserConsole.Log(context.Response, new { msg = "test" });

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var html = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Contains("console.log", html);
        Assert.Contains("msg", html);
    }

    [Fact]
    public async Task DoesNotWriteConsoleLogInProduction()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "Development", "UAT" };
        ConsoleOutputSettings.CurrentEnvironment = "Production";

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await BrowserConsole.Log(context.Response, "test");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var html = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.True(string.IsNullOrWhiteSpace(html));
    }

    [Fact]
    public async Task WritesConsoleTable()
    {
        ConsoleOutputSettings.AllowedEnvironments = new[] { "Development" };
        ConsoleOutputSettings.CurrentEnvironment = "Development";

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var data = new[]
        {
            new { Id = 1, Name = "Alice" },
            new { Id = 2, Name = "Bob" }
        };

        await BrowserConsole.Table(context.Response, data);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var html = new StreamReader(context.Response.Body).ReadToEnd();

        Assert.Contains("console.table", html);
        Assert.Contains("Alice", html);
        Assert.Contains("Bob", html);
    }
}