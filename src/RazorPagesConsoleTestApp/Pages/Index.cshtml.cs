using DotNetCoreOutputToConsole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesConsoleTestApp.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        await BrowserConsole.Log(Response, "Razor Page Loaded");
        await BrowserConsole.Info(Response, new { Page = "Index", Now = DateTime.UtcNow });
        await BrowserConsole.Warn(Response, "Razor warning message");
        await BrowserConsole.Error(Response, new { Error = "Razor page error" });
        await BrowserConsole.Trace(Response, "Trace from Razor IndexModel");
        await BrowserConsole.Table(Response, new[]
        {
                new { Id = 1, Name = "Razor Row 1" },
                new { Id = 2, Name = "Razor Row 2" }
            });
    }
}
