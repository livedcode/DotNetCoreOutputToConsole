using DotNetCoreOutputToConsole.Config;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;


namespace DotNetCoreOutputToConsole;

/// <summary>
/// Helper for writing to browser console safely (DEV/UAT only).
/// </summary>
public static class BrowserConsole
{
    public static Task Log(HttpResponse response, object msg)
        => Write(response, "log", msg);

    public static Task Info(HttpResponse response, object msg)
        => Write(response, "info", msg);

    public static Task Warn(HttpResponse response, object msg)
        => Write(response, "warn", msg);

    public static Task Error(HttpResponse response, object msg)
        => Write(response, "error", msg);

    public static Task Trace(HttpResponse response, object msg)
        => Write(response, "trace", msg);

    public static Task Table(HttpResponse response, object msg)
        => Write(response, "table", msg);

    private static async Task Write(HttpResponse response, string fn, object msg)
    {
        if (!ConsoleOutputSettings.IsEnabled)
            return;

        // 1. Serialize to raw JSON
        var json = JsonSerializer.Serialize(msg);

        // 2. Escape JSON for safe embedding inside a JS string
        var js = EscapeForJs(json);

        // 3. Build the script
        var script = $"<script>console.{fn}(JSON.parse('{js}'));</script>";

        // 4. Only set ContentType if possible
        if (!response.HasStarted)
        {
            response.ContentType = "text/html";
        }

        // 5. Write script (safe no-op if impossible)
        try
        {
            await response.WriteAsync(script);
        }
        catch
        {
            // swallow silently — this library must NEVER break the real response
        }
    }

    private static string EscapeForJs(string json)
    {
        return json
            .Replace("\\", "\\\\")
            .Replace("'", "\\'")
            .Replace("\r", "")
            .Replace("\n", " ")
            .Replace("</script>", "<\\/script>");
    }
}
