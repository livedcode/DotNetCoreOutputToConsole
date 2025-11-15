using DotNetCoreOutputToConsole;
using Microsoft.AspNetCore.Mvc;
using MvcConsoleTestApp.Models;
using System.Diagnostics;

namespace MvcConsoleTestApp.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async  Task<IActionResult> Index()
    {
        await BrowserConsole.Log(Response, "MVC Home loaded");
        await BrowserConsole.Info(Response, new { Controller = "Home", Action = "Index" });
        await BrowserConsole.Warn(Response, "MVC warning");
        await BrowserConsole.Error(Response, new { Error = "MVC error message" });
        await BrowserConsole.Trace(Response, "Trace from MVC");
        await BrowserConsole.Table(Response, new[]
        {
                new { Id = 1, Name = "MVC Row 1" },
                new { Id = 2, Name = "MVC Row 2" }
            });
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
